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

    public class LoadingInfo
    {
        public System.Type type;

        public LoadingInfo(System.Type type)
        {
            this.type = type;
        }
    }

    public class LoadingInfoPanel<T> : LoadingInfo where T : UIPanel
    {
        public Action<T> callback;
        public UIShowMode showMode;
        public UILevel level;

        public LoadingInfoPanel(Action<T> callback, UIShowMode showMode, UILevel level) : base(typeof(T))
        {
            this.callback = callback;
            this.showMode = showMode;
            this.level = level;
        }
    }

    public class LoadingInfoWidget<T> : LoadingInfo where T : UIWidget
    {
        public Action<T> callback;

        public LoadingInfoWidget(Action<T> callback) : base(typeof(T))
        {
            this.callback = callback;
        }
    }

    public class UIMgr
    {
        private Canvas _root;

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

        public void Init()
        {
            _panelLevel = new List<UIPanel>(32);
            _panelOpStack = new List<UIPanel>(32);
            _uiWidgets = new List<UIWidget>(64);
            _loadingPanels = new List<LoadingInfo>(16);
            _loadingWidgets = new List<LoadingInfo>(16);

            _root = Game.Instance.Canvas;
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

        public void Show<T>(Action<T> callback, UIShowMode showMode, UILevel level) where T : UIPanel
        {
            if (TryGetPanelIndexInStack(out int _, out T p))
            {
                if (p.IsActive)
                {
                    callback?.Invoke(p);
                }
                else
                {
                    // Show(p, callback, showMode, level);
                }
                return;
            }

            if (TryGetLoadingPanel<T>(out LoadingInfoPanel<T> oldInfo))
            {
                if (callback != null)
                    oldInfo.callback += callback;
                oldInfo.showMode = showMode;
                oldInfo.level = level;
                return;
            }

            // LoadingInfo
            LoadingInfoPanel<T> loadingInfo = new LoadingInfoPanel<T>(callback, showMode, level);
            _loadingPanels.Add(loadingInfo);

            // Load
            OnLoadPanelDone<T>(p =>
            {
                if (TryGetLoadingPanel<T>(out LoadingInfoPanel<T> info))
                {
                    _loadingPanels.Remove(info);
                    p.level = info.level;
                    p.showMode = info.showMode;

                    // Last op panel
                    bool isFirstPanel = !TryGetLastOpPanel(out var lastOpPanel);

                    // Level
                    int level = InsertAtLevel(p, info.level);
                    p.transform.SetSiblingIndex(level);

                    // Operation stack
                    _panelOpStack.Add(p);

                    // Show mode
                    if (!isFirstPanel)
                    {
                        if (info.showMode == UIShowMode.Cover)
                        {
                            // lastPanel.CoverBy(p);
                        }
                        else if (info.showMode == UIShowMode.Push)
                        {
                            // Hide last
                            Hide(lastOpPanel, null);
                        }
                        else if (info.showMode == UIShowMode.Replace)
                        {
                            // Close last
                            p.showMode = lastOpPanel.showMode;
                            Close(lastOpPanel);
                        }
                    }
                    // 自己是第一个打开的界面
                    else
                    {
                    }

                    // New panel op
                    p.DoShowAnimation(null);

                    // Callback
                    info.callback?.Invoke(p);
                }
            });
        }

        public void Show(UIPanel p)
        {
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

        public void Hide(UIPanel panel, Action<UIPanel> callback)
        {
            if (panel == null)
                return;

            panel.OnHide();
            panel.DoHidingAnimation(p =>
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
            panel.OnHide();
            panel.OnClose();
            panel.DoHidingAnimation(p =>
            {
                // 清除panel
                _panelLevel.Remove(p);
                _panelOpStack.Remove(p);
                // 销毁panel
                p.OnDrop();
                GameObject.Destroy(p.gameObject);
            });
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

        private void RemoveLoadingPanel()
        {
        }

        private void RemoveLoadingWidget()
        {
        }

        /// <returns>Panel's level index</returns>
        private int InsertAtLevel(UIPanel p, UILevel level)
        {
            int lastIndex = FindLastIndexAtLevel(level);
            _panelLevel.Insert(lastIndex + 1, p);
            return lastIndex + 1;
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
            callback(p);
        }
    }
}