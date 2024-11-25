using System;
using Jam.Core;
using System.Collections.Generic;
using System.Reflection;
using cfg;
using Jam.Runtime.Asset;
using Jam.Runtime.Constant;
using Jam.Runtime.ObjectPool;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Jam.Runtime.UI_
{

    public class UIMgr : IMgr, ITickable
    {
        // Id -> Type map
        private Dictionary<UIPanelId, Type> _idToType;
        private Dictionary<Type, UIPanelId> _typeToId;

        // Res load
        private Queue<UIPanel> _recycleQueue;

        // Pool
        private IObjectPool<UIPanelObject> _pool;

        // 
        private HashSet<UIPanelId> _waitingReleaseWhenLoading;
        private Dictionary<UIPanelId, UIOpenPanelInfo> _loadingPanels;
        private List<UIPanel> _panels;       // 根据层级排序
        private List<UIPanel> _panelOpStack; // 操作序列

        // Settings
        [SerializeField] private Canvas _root;
        [SerializeField] private float _poolAutoReleaseInterval = 60f;
        [SerializeField] private int _poolCapacity = 16;
        [SerializeField] private float _poolExpireTime = 60f;

        public UIMgr()
        {
            _idToType = new Dictionary<UIPanelId, Type>(64);
            _typeToId = new Dictionary<Type, UIPanelId>(64);
            _recycleQueue = new Queue<UIPanel>(16);
            _waitingReleaseWhenLoading = new HashSet<UIPanelId>(16);
            _loadingPanels = new Dictionary<UIPanelId, UIOpenPanelInfo>(16);
            _panels = new List<UIPanel>(32);
            _panelOpStack = new List<UIPanel>(32);
            LoadTypeIdMap();
        }

        public void Init()
        {
            _root = G.Instance.UICanvas;
            _pool = G.ObjectPool.CreateSingleSpawnObjectPool<UIPanelObject>(
                "UIPanelPool", _poolCapacity, _poolExpireTime);
        }

        public void Shutdown(bool isAppQuit)
        {
        }

        // TODO: 

        // TODO: Use base comp to Update
        public void Tick(float dt)
        {
            while (_recycleQueue.Count > 0)
            {
                UIPanel p = _recycleQueue.Dequeue();
                p.OnRecycle();
                _pool.Unspawn(p);
            }

            foreach (var p in _panels)
            {
                p.Tick(dt);
            }
        }

        public void Open(UIPanelId id, UIShowMode showMode, UILevel level)
        {
            Open(id, showMode, level, null, null);
        }

        public void Open(UIPanelId id,
                         UIShowMode showMode = UIShowMode.None,
                         UILevel level = UILevel.None,
                         Action<UIPanel> callback = null,
                         object userData = null)
        {
            // 已经打开过的界面不处理
            if (TryGet(id, out UIPanel panel))
                return;

            // 正在加载中的也不处理
            if (TryGetLoadingInfo(id, out var openInfo))
            {
                openInfo.UpdateInfo(showMode, level, callback, userData);
                return;
            }

            GetModeAndLevel(id, ref showMode, ref level);
            // 加载资源
            UIPanelObject poolObj = _pool.Spawn(GetUIRealAssetPath(id)); // 从配置表获得string减少gc
            if (poolObj == null)
            {
                UIOpenPanelInfo info = UIOpenPanelInfo.Create(id, showMode, level, callback, userData);
                _loadingPanels.Add(id, info);
                G.Asset.Load(GetUIRealAssetPath(id), typeof(GameObject), LoadAssetCallback, info);
            }
            else
            {
                OpenImpl((UIPanel)poolObj.Target, showMode, level, false, callback, userData);
            }
        }

        public void Close(UIPanelId id)
        {
            if (IsLoading(id))
            {
                _waitingReleaseWhenLoading.Add(id);
                _loadingPanels.Remove(id);
                return;
            }

            // 没有这个界面
            if (!TryGet(id, out UIPanel panel))
                return;
            Close(panel);
        }

        public void Close(UIPanel panel)
        {
            if (panel == null)
                throw new Exception("UI panel is invalid.");

            if (panel.IsVisible)
            {
                panel.OnHide();
                panel.OnClose();
                panel.PlayHidingAnim(p =>
                {
                    CloseImpl(p);
                });
            }
            else
            {
                panel.OnClose();
                CloseImpl(panel);
            }
        }

        public void Back()
        {
            if (_panelOpStack.Count <= 1)
                return;
            UIPanel panel = _panelOpStack[^1];
            Close(panel);
        }

        public void Show(UIPanelId id)
        {
            // 没有这个界面
            if (!TryGet(id, out UIPanel panel))
                return;
            Show(panel);
        }

        public void Show(UIPanel panel)
        {
            if (panel == null)
                throw new Exception("UI panel is invalid.");

            if (panel.IsVisible)
                return;
            panel.gameObject.SetActive(true);
            panel.PlayShowingAnim(p =>
            {
                p.OnShow();
            });
        }

        public void Hide(UIPanelId id, Action<UIPanel> callback = null)
        {
            // 没有这个界面
            if (!TryGet(id, out UIPanel panel))
                return;
            Hide(panel, callback);
        }

        public void Hide(UIPanel panel, Action<UIPanel> callback = null)
        {
            if (panel == null)
                throw new Exception("UI panel is invalid.");

            if (!panel.IsVisible)
                return;

            panel.OnHide();
            panel.PlayHidingAnim(p =>
            {
                callback?.Invoke(p);
                p.gameObject.SetActive(false);
            });
        }

        public UIPanel Get(UIPanelId id)
        {
            foreach (var panel in _panels)
            {
                if (panel.Id == id)
                    return panel;
            }
            return null;
        }

        public T Get<T>() where T : UIPanel
        {
            return Get(_typeToId[typeof(T)]) as T;
        }

        public bool TryGet(UIPanelId id, out UIPanel panel)
        {
            panel = Get(id);
            return panel != null;
        }

        public bool TryGet<T>(out T panel) where T : UIPanel
        {
            panel = Get<T>();
            return panel != null;
        }

        public bool Has(UIPanelId id)
        {
            return Get(id) != null;
        }

        public bool Has<T>() where T : UIPanel
        {
            return Get<T>() != null;
        }

        public bool IsLoading(UIPanelId id)
        {
            return _loadingPanels.ContainsKey(id);
        }

        public bool TryGetLoadingInfo(UIPanelId id, out UIOpenPanelInfo info)
        {
            return _loadingPanels.TryGetValue(id, out info);
        }

        // Helpers
        // Open & Close
        private void OpenImpl(UIPanel panel,
                              UIShowMode showMode,
                              UILevel level,
                              bool isNew,
                              Action<UIPanel> callback,
                              object userData)
        {
            panel.showMode = showMode;
            panel.level = level;
            // 上一个操作的Panel
            bool isFirstPanel = !TryGetLastOpPanel(out var lastOpPanel);
            // 层级
            InsertAtLevel(panel, level);
            // 操作栈
            _panelOpStack.Add(panel);
            // 如果不是第一个打开的界面
            if (!isFirstPanel)
            {
                // 打开方式
                if (showMode is UIShowMode.Cover or UIShowMode.None)
                {
                }
                else if (showMode == UIShowMode.Push)
                {
                    // Hide last
                    Hide(lastOpPanel);
                }
                else if (showMode == UIShowMode.Replace)
                {
                    // Close last
                    panel.showMode = lastOpPanel.showMode;
                    lastOpPanel.showMode = UIShowMode.None;
                    Close(lastOpPanel);
                }
            }
            //
            if (isNew)
                panel.OnInit();
            panel.OnOpen(userData);
            panel.gameObject.SetActive(true);
            panel.OnShow();
            panel.PlayShowingAnim();
            callback?.Invoke(panel);
        }

        private void CloseImpl(UIPanel panel)
        {
            UIShowMode showMode = panel.showMode;
            if (showMode == UIShowMode.Push)
            {
                // 如果自己是最后一个操作的panel
                if (IsLastOpPanel(panel))
                {
                    // 显示操作序列中前一个panel
                    if (TryGetOpPanelBefore(panel, out var lastOpPanel))
                    {
                        Show(lastOpPanel);
                    }
                }
                else
                {
                    // 操作序列中后一个panel
                    if (TryGetOpPanelAfter(panel, out var nextOpPanel))
                    {
                        // 继承showMode, 然后把自己关了
                        nextOpPanel.showMode = panel.showMode;
                    }
                }
            }

            panel.gameObject.SetActive(false);
            _panels.Remove(panel);
            _panelOpStack.Remove(panel);
            _recycleQueue.Enqueue(panel);
        }

        // Level & OpStack
        private int InsertAtLevel(UIPanel panel, UILevel level)
        {
            int lastIndex = FindLastIndexAtLevel(level);
            _panels.Insert(lastIndex + 1, panel);
            int insertIndex = lastIndex + 1;
            panel.transform.SetSiblingIndex(insertIndex);
            return insertIndex;
        }

        private int FindLastIndexAtLevel(UILevel level)
        {
            int checkLevel = (int)level;
            for (int i = _panels.Count - 1; i >= 0; i--)
            {
                int itLevel = (int)_panels[i].level;
                if (checkLevel >= itLevel)
                {
                    return i;
                }
            }
            return -1;
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

        private bool IsLastOpPanel(UIPanel p)
        {
            if (TryGetLastOpPanel(out UIPanel last))
            {
                return last == p;
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

        // Config
        private UIPanelConfig GetPanelConfig(UIPanelId id)
        {
            return G.Cfg.TbUIPanelConfig.Get(id);
        }

        private void GetModeAndLevel(UIPanelId id, ref UIShowMode showMode, ref UILevel level)
        {
            UIPanelConfig cfg = GetPanelConfig(id);
            showMode = showMode != UIShowMode.None ? showMode : cfg.ShowMode;
            level = level != UILevel.None ? level : cfg.Level;
        }

        private string GetUIRealAssetPath(UIPanelId id)
        {
            UIPanelConfig cfg = G.Cfg.TbUIPanelConfig.Get(id);
            return AssetPath.UIPanel(cfg.AssetName);
        }

        // Load res callback
        private void LoadAssetCallback(AssetHandleWrap wrap)
        {
            UIOpenPanelInfo openInfo = (UIOpenPanelInfo)wrap.UserData;
            if (openInfo == null)
                throw new Exception("Open UI panel info is invalid.");
            if (wrap.IsSuccess)
            {
                if (_waitingReleaseWhenLoading.Contains(openInfo.PanelId))
                {
                    _waitingReleaseWhenLoading.Remove(openInfo.PanelId);
                    openInfo.Dispose();
                    ReleaseUI(wrap.Id, null);
                    return;
                }

                _loadingPanels.Remove(openInfo.PanelId);
                UIPanel panel = GameObject.Instantiate((GameObject)wrap.Asset, _root.transform)
                                          .GetComponent<UIPanel>();
                UIPanelObject poolObj = UIPanelObject.Create(wrap.AssetPath, wrap.Id, panel);
                _pool.Register(poolObj, true);
                OpenImpl(panel, openInfo.ShowMode, openInfo.Level, true, openInfo.Callback, openInfo.UserData);
                openInfo.Dispose();
            }
            else
            {
                if (_waitingReleaseWhenLoading.Contains(openInfo.PanelId))
                {
                    _waitingReleaseWhenLoading.Remove(openInfo.PanelId);
                    return;
                }
                _loadingPanels.Remove(openInfo.PanelId);
                openInfo.Dispose();
                string appendErrorMessage = Util.Text.Format("Load UI panel failure, asset name '{0}'", wrap.AssetPath);
                throw new Exception(appendErrorMessage);
            }
        }

        // Others
        private void ReleaseUI(int assetHandleId, object panelGo)
        {
            G.Asset.Unload(assetHandleId);
            GameObject.Destroy((Object)panelGo);
        }

        private void LoadTypeIdMap()
        {
            foreach (var type in Util.Assembly.GetTypes())
            {
                if (type.IsDefined(typeof(UIPanelAttribute), false))
                {
                    UIPanelAttribute attr = (UIPanelAttribute)type.GetCustomAttribute(typeof(UIPanelAttribute), false);
                    _idToType.Add(attr.Id, type);
                    _typeToId.Add(type, attr.Id);
                }
            }
        }
    }

}