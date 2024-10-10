using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Temp.NewUI
{

    public enum UIShowMode
    {
        Cover,   // 覆盖显示
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

    public class LoadingInfoPanel<T> : LoadingInfo where T : NewUIPanel
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

    public class LoadingInfoWidget<T> : LoadingInfo where T : NewUIWidget
    {
        public Action<T> callback;

        public LoadingInfoWidget(Action<T> callback) : base(typeof(T))
        {
            this.callback = callback;
        }
    }

    public class NewUIMgr
    {
        // 层级
        private List<NewUIPanel> _panelLevel;
        // 操作序列
        private List<NewUIPanel> _panelStack;
        // 控件
        private List<NewUIWidget> _uiWidgets;
        // 正在加载中的面板
        private List<LoadingInfo> _loadingPanels;
        // 正在加载中的Widget
        private List<LoadingInfo> _loadingWidgets;

        public void Init()
        {
            _panelLevel = new List<NewUIPanel>(32);
            _panelStack = new List<NewUIPanel>(32);
            _uiWidgets = new List<NewUIWidget>(64);
            _loadingPanels = new List<LoadingInfo>(16);
            _loadingWidgets = new List<LoadingInfo>(16);
        }

        public void Shutdown()
        {
            _panelLevel.Clear();
            _panelLevel = null;

            // TODO: Dispose all panels
            foreach (var p in _panelStack)
            {
            }
            _panelStack.Clear();
            _panelStack = null;

            // TODO: Dispose all widgets
            foreach (var w in _uiWidgets)
            {
            }
            _uiWidgets.Clear();
            _uiWidgets = null;
        }

        public void Show<T>(System.Action<T> callback, UIShowMode showMode, UILevel level) where T : NewUIPanel
        {
            if (TryGetLoadingPanel<T>(out LoadingInfoPanel<T> oldInfo))
            {
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

                    // Level
                    int levelIndex = InsertAtLevel(p, info.level);
                    int lastPanelIndex = levelIndex - 1;

                    // Operation stack
                    _panelStack.Add(p);

                    // Show mode
                    if (lastPanelIndex >= 0)
                    {
                        NewUIPanel lastPanel = _panelLevel[lastPanelIndex];
                        if (info.showMode == UIShowMode.Cover)
                        {
                            lastPanel.CoverBy(p);
                        }
                        else if (info.showMode == UIShowMode.Push)
                        {
                            lastPanel.Hide();
                        }
                        else if (info.showMode == UIShowMode.Replace)
                        {
                            p.showMode = lastPanel.showMode;
                            CloseImpl(lastPanel);
                        }
                    }

                    // New panel op
                    p.Show();

                    // Callback
                    info.callback?.Invoke(p);
                }
            });
        }

        public void Hide<T>() where T : NewUIPanel
        {
        }

        public void Close<T>()
        {
        }

        public void Get<T>(System.Action<T> callback)
        {
        }

        public void HideLast()
        {
        }

        public void HideAll(bool reserveState)
        {
        }

        public void ReshowAll()
        {
        }

        // change level prev T
        public void MoveLevelPrev<T>() where T : NewUIPanel
        {
        }

        // change level next T
        public void MoveLevelNext<T>() where T : NewUIPanel
        {
        }

        public void Tick(float dt)
        {
        }

        // Impl
        private void CloseImpl(NewUIPanel p)
        {
        }

        // Helpers
        private bool TryFindPanelIndexInStack<T>(out int index, out T panel) where T : NewUIPanel
        {
            index = -1;
            panel = null;
            for (int i = 0; i < _panelStack.Count; i++)
            {
                if (_panelStack[i] is T)
                {
                    index = i;
                    panel = _panelStack[i] as T;
                    return true;
                }
            }
            return false;
        }

        private bool TryFindPanelIndexInLevel<T>(out int index, out T panel) where T : NewUIPanel
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

        private bool TryGetLoadingPanel<T>(out LoadingInfoPanel<T> panelInfo) where T : NewUIPanel
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

        private bool TryGetLoadingWidget<T>(out LoadingInfoWidget<T> widgetInfo) where T : NewUIWidget
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
        private int InsertAtLevel(NewUIPanel p, UILevel level)
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
        private void OnLoadPanelDone<T>(Action<T> callback) where T : NewUIPanel
        {
            string path = $"Art/UI/Panels/{typeof(T).Name}";
            var handle = Addressables.LoadAssetAsync<GameObject>(path);
            // Load done
            handle.Completed += resHandle =>
            {
                if (resHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject go = resHandle.Result;
                    T p = go.GetComponent<T>();
                    callback?.Invoke(p);
                }
                else
                {
                }
            };
        }
    }

}