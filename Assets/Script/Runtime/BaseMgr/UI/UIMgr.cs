using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using J.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace J.Runtime.UI
{
    public enum UIShowMode
    {
        None,
        Cover,   // 覆盖显示, 一个panel只会被覆盖一次
        Push,    // 隐藏最近打开，当关闭时重新显示
        Replace, // 关闭最近打开
    }

    public enum UILevel
    {
        None,
        Bottom,
        Mid,
        Top,
        Debug,
    }

    public class UIMgr
    {
        private Canvas _root;

        // 配置
        private static UIConfig _config;

        // 层级
        private List<UIPanel> _panelLevel;

        // 操作序列, 打开界面的顺序
        private List<UIPanel> _panelOpStack;

        // 控件
        private List<UIWidget> _uiWidgets;

        // 正在加载中的面板
        private List<LoadingInfo> _loadingPanels;

        // 正在加载中的Widget
        private List<LoadingInfo> _loadingWidgets;

        public static UIConfig Config => _config;

        public void Init()
        {
            _config = new UIConfig();
            _panelLevel = new List<UIPanel>(32);
            _panelOpStack = new List<UIPanel>(32);
            _uiWidgets = new List<UIWidget>(64);
            _loadingPanels = new List<LoadingInfo>(16);
            _loadingWidgets = new List<LoadingInfo>(16);

            _root = GameObject.Find("Canvas").GetComponent<Canvas>();
        }

        public void Shutdown()
        {
            _panelLevel.Clear();
            _panelLevel = null;

            // TODO: Dispose all panels
            foreach (var p in _panelOpStack)
            {
            }
            _panelOpStack.Clear();
            _panelOpStack = null;

            // TODO: Dispose all widgets
            foreach (var w in _uiWidgets)
            {
            }
            _uiWidgets.Clear();
            _uiWidgets = null;
        }

        public void Show<T>(Action<T> callback = null, UIShowMode showMode = UIShowMode.None,
                            UILevel level = UILevel.None) where T : UIPanel
        {
            // Panel已经存在
            if (TryGetPanel(out T p))
            {
                if (p.IsVisible)
                {
                    callback?.Invoke(p);
                }
                else
                {
                    ReShow(p);
                }
                return;
            }

            // Panel正在加载
            if (TryGetLoadingPanel<T>(out LoadingInfoPanel<T> oldInfo))
            {
                if (callback != null)
                    oldInfo.callback += callback;
                oldInfo.UpdateConfig(showMode, level);
                return;
            }

            // Panel的加载信息
            LoadingInfoPanel<T> loadingInfo = new LoadingInfoPanel<T>(callback, showMode, level);
            _loadingPanels.Add(loadingInfo);

            // 加载
            OnLoadPanelDone<T>(p =>
            {
                // 获取加载信息
                if (TryGetLoadingPanel<T>(out LoadingInfoPanel<T> info))
                {
                    _loadingPanels.Remove(info);
                    p.level = info.Level;
                    p.showMode = info.ShowMode;

                    // 上一个操作的Panel
                    bool isFirstPanel = !TryGetLastOpPanel(out var lastOpPanel);

                    // 层级
                    int level = InsertAtLevel(p, info.Level);

                    // 操作栈
                    _panelOpStack.Add(p);

                    // 如果不是第一个打开的界面
                    if (!isFirstPanel)
                    {
                        // 打开方式
                        if (info.ShowMode == UIShowMode.Cover)
                        {
                            // lastPanel.CoverBy(p);
                        }
                        else if (info.ShowMode == UIShowMode.Push)
                        {
                            // Hide last
                            Hide(lastOpPanel);
                        }
                        else if (info.ShowMode == UIShowMode.Replace)
                        {
                            // Close last
                            p.showMode = lastOpPanel.showMode;
                            lastOpPanel.showMode = UIShowMode.None;
                            Close(lastOpPanel);
                        }
                    }
                    // 自己是第一个打开的界面
                    else
                    {
                    }

                    // New panel op
                    p.OnShow();
                    p.PlayShowingAnim();

                    // Callback
                    info.callback?.Invoke(p);
                }
            });
        }

        // Panel 已经存在, 只是隐藏了
        public void ReShow(UIPanel panel)
        {
            if (panel.IsVisible)
                return;
            panel.PlayShowingAnim(p =>
            {
                p.gameObject.SetActive(true);
                p.OnShow();
            });
        }

        public void Hide<T>(Action<UIPanel> callback) where T : UIPanel
        {
            if (TryGet(out T panel, out bool isLoading))
            {
                Hide(panel, callback);
                return;
            }

            if (isLoading)
            {
                Get<T>(p => Hide(p, callback));
                return;
            }

            JLog.Warning($"Panel {typeof(T).Name} not found");
        }

        public void Hide(UIPanel panel, Action<UIPanel> callback = null)
        {
            if (panel == null)
                return;

            panel.OnHide();
            panel.PlayHidingAnim(p =>
            {
                callback?.Invoke(p);
                p.gameObject.SetActive(false);
            });
        }

        public void Close<T>() where T : UIPanel
        {
            if (TryGet(out T panel, out bool isLoading))
            {
                Close(panel);
                return;
            }

            if (isLoading)
            {
                Get<T>(Close);
            }
        }

        public void Close(UIPanel panel)
        {
            if (panel == null)
                return;

            // 如果这个panel正在显示
            if (panel.IsVisible)
            {
                panel.OnHide();
                panel.OnClose();
                panel.PlayHidingAnim(p =>
                {
                    CloseImpl(p);
                });
            }
            // 关闭了隐藏的panel
            else
            {
                CloseImpl(panel);
            }
        }

        private void CloseImpl(UIPanel p)
        {
            UIShowMode showMode = p.showMode;
            if (showMode == UIShowMode.Push)
            {
                // 如果自己是顶层panel
                if (IsLastOpPanel(p))
                {
                    // 显示操作序列中前一个panel
                    if (TryGetOpPanelBefore(p, out var lastOpPanel))
                    {
                        ReShow(lastOpPanel);
                    }
                }
                else
                {
                    // 操作序列中后一个panel
                    if (TryGetOpPanelAfter(p, out var nextOpPanel))
                    {
                        // 继承showMode,然后把自己关了
                        nextOpPanel.showMode = p.showMode;
                    }
                }
            }

            // 清除panel
            _panelLevel.Remove(p);
            _panelOpStack.Remove(p);

            // 销毁panel
            p.OnDrop();
            GameObject.Destroy(p.gameObject);
        }

        public void Get<T>(Action<T> callback) where T : UIPanel
        {
            if (TryGet(out T panel, out bool isLoading))
            {
                callback(panel);
                return;
            }

            if (isLoading)
            {
                TryGetLoadingPanel(out LoadingInfoPanel<T> info);
                info.callback += callback;
                return;
            }

            JLog.Warning("Panel not found");
        }

        public bool TryGet<T>(out T panel, out bool isLoading) where T : UIPanel
        {
            isLoading = false;
            panel = null;
            if (TryGetPanelIndexInStack(out int index, out T p))
            {
                panel = p;
                return true;
            }

            if (TryGetLoadingPanel(out LoadingInfoPanel<T> info))
            {
                isLoading = true;
                return false;
            }
            return false;
        }

        public void HideLast()
        {
            if (TryGetLastOpPanel(out var lastOpPanel))
            {
                Hide(lastOpPanel, null);
            }
        }

        public void CloseLast()
        {
            if (TryGetLastOpPanel(out var lastOpPanel))
            {
                Close(lastOpPanel);
            }
        }

        // public void HideAll(bool reserveState)
        // {
        // }
        //
        // public void ReshowAll()
        // {
        // }

        // change level prev T
        public void MoveLevelPrev<T>() where T : UIPanel
        {
        }

        // change level next T
        public void MoveLevelNext<T>() where T : UIPanel
        {
        }

        public void Tick(float dt)
        {
            for (int i = _panelOpStack.Count - 1; i >= 0; i--)
            {
                var p = _panelOpStack[i];
                p.Tick(dt);
            }
        }

        // Helpers
        private bool TryGetPanel<T>(out T panel) where T : UIPanel
        {
            panel = null;
            for (int i = 0; i < _panelOpStack.Count; i++)
            {
                if (_panelOpStack[i] is T)
                {
                    panel = _panelOpStack[i] as T;
                    return true;
                }
            }
            return false;
        }

        private bool TryGetPanelIndexInStack<T>(out int index, out T panel) where T : UIPanel
        {
            index = -1;
            panel = null;
            for (int i = 0; i < _panelOpStack.Count; i++)
            {
                if (_panelOpStack[i] is T)
                {
                    index = i;
                    panel = _panelOpStack[i] as T;
                    return true;
                }
            }
            return false;
        }

        private bool TryGetPanelIndexInLevel<T>(out int index, out T panel) where T : UIPanel
        {
            index = -1;
            panel = null;
            for (int i = 0; i < _panelLevel.Count; i++)
            {
                if (_panelLevel[i] is T)
                {
                    index = i;
                    panel = _panelLevel[i] as T;
                    return true;
                }
            }
            return false;
        }

        private bool TryGetLastOpPanel(out UIPanel panel)
        {
            panel = null;
            if (_panelOpStack.Count > 0)
            {
                panel = _panelOpStack[^1];
                return true;
            }
            return false;
        }

        private bool TryGetOpPanelBefore(UIPanel panel, out UIPanel outPanel)
        {
            int index = 0;
            for (int i = 0; i < _panelOpStack.Count; i++)
            {
                if (_panelOpStack[i] == panel)
                {
                    index = i;
                }
            }
            int beforeIndex = index - 1;
            if (beforeIndex >= 0)
            {
                outPanel = _panelOpStack[beforeIndex];
                return true;
            }
            outPanel = null;
            return false;
        }

        private bool TryGetOpPanelAfter(UIPanel panel, out UIPanel outPanel)
        {
            int index = 0;
            for (int i = 0; i < _panelOpStack.Count; i++)
            {
                if (_panelOpStack[i] == panel)
                {
                    index = i;
                }
            }
            int afterIndex = index + 1;
            if (afterIndex < _panelOpStack.Count)
            {
                outPanel = _panelOpStack[afterIndex];
                return true;
            }
            outPanel = null;
            return false;
        }

        private bool TryGetLoadingPanel<T>(out LoadingInfoPanel<T> panelInfo) where T : UIPanel
        {
            panelInfo = default;
            for (int i = 0; i < _loadingPanels.Count; i++)
            {
                if (_loadingPanels[i].type == typeof(T))
                {
                    panelInfo = _loadingPanels[i] as LoadingInfoPanel<T>;
                    return true;
                }
            }
            return false;
        }

        private bool TryGetLoadingWidget<T>(out LoadingInfoWidget<T> widgetInfo) where T : UIWidget
        {
            widgetInfo = default;
            for (int i = 0; i < _loadingWidgets.Count; i++)
            {
                if (_loadingWidgets[i].type == typeof(T))
                {
                    widgetInfo = _loadingWidgets[i] as LoadingInfoWidget<T>;
                    return true;
                }
            }
            return false;
        }

        private bool IsLastOpPanel(UIPanel p)
        {
            if (TryGetLastOpPanel(out UIPanel last))
            {
                return last == p;
            }
            return false;
        }

        /// <returns>Panel's level index</returns>
        private int InsertAtLevel(UIPanel p, UILevel level)
        {
            int lastIndex = FindLastIndexAtLevel(level);
            _panelLevel.Insert(lastIndex + 1, p);
            int insertIndex = lastIndex + 1;
            p.transform.SetSiblingIndex(insertIndex);
            return insertIndex;
        }

        private int FindLastIndexAtLevel(UILevel level)
        {
            int checkLevel = (int)level;
            for (int i = _panelLevel.Count - 1; i >= 0; i--)
            {
                int itLevel = (int)_panelLevel[i].level;
                if (checkLevel >= itLevel)
                {
                    return i;
                }
            }
            return -1;
        }

        // TODO: Add fail callback
        private async UniTaskVoid OnLoadPanelDone<T>(Action<T> callback) where T : UIPanel
        {
            string path = $"Assets/Artwork/UI/Panels/{typeof(T).Name}.prefab";
            var prefab = await Game.ResMgr.Load<GameObject>(path);
            if (prefab == null)
                return;

            GameObject pGo = GameObject.Instantiate(prefab, _root.transform);
            T p = pGo.GetComponent<T>();
            p.OnLoad();
            callback(p);
        }
    }
}