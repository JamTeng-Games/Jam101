using Jam.Cfg;
using UnityEngine;
using System;
using System.Collections.Generic;
using Jam.Core;
using Jam.Runtime.Asset;
using Jam.Runtime.ObjectPool;

namespace Jam.Runtime.UI_
{

    public partial class UIMgr
    {
        // Id -> Type map
        private Dictionary<UIWidgetId, Type> _idToWidgetType;
        private Dictionary<Type, UIWidgetId> _widgetTypeToId;

        // Res load
        // private Queue<UIWidget> _widgetRecycleQueue;

        // [SerializeField] private float _widgetPoolAutoReleaseInterval = 60f;
        [SerializeField] private int _widgetPoolCapacity = 16;
        [SerializeField] private float _widgetPoolExpireTime = 60f;

        private int _widgetGenId = 0;
        private Dictionary<int, UIWidget> _widgets;
        private Dictionary<UIWidgetId, List<UIWidget>> _widgetPool;
        private Transform _widgetPoolRoot;

        private void InitWidget()
        {
            _widgetPoolRoot = new GameObject("WidgetPool").transform;
            _widgetPoolRoot.SetParent(G.Instance.UICanvas.transform);
            _widgetPoolRoot.gameObject.SetActive(false);
            _widgetPool = new Dictionary<UIWidgetId, List<UIWidget>>();
        }

        public int CreateWidget(UIWidgetId widgetType,
                                IWidgetOwner owner,
                                Transform parentTrans,
                                Action<UIWidget> callback,
                                object userData = null)
        {
            int widgetId = ++_widgetGenId;
            // 加载资源
            UIWidget widget = RentWidgetFromPool(widgetType);
            if (widget == null)
            {
                UIWidgetOpenInfo info = UIWidgetOpenInfo.Create(widgetId, widgetType, owner, parentTrans, callback, userData);
                G.Asset.Load(GetUIWidgetRealAssetPath(widgetType), typeof(GameObject), LoadWidgetAssetCallback, info);
                if (info.Id == 0)
                {
                    JLog.Error($"CreateWidget id == 0 {widgetType}");
                }
                JLog.Debug($"CreateWidget {widgetType} return null id {info.Id}");
                return widgetId;
            }
            else
            {
                OpenWidgetImpl(widgetId, widget, owner, parentTrans, false, callback, userData);
                JLog.Debug($"CreateWidget {widgetType} return id {widget.Id}");
                return widget.Id;
            }

            // int widgetId = ++_widgetGenId;
            // // 加载资源
            // UIWidgetObject poolObj = _widgetPool.Spawn(GetUIWidgetRealAssetPath(widgetType) + widgetId.ToString());
            // if (poolObj == null)
            // {
            //     UIWidgetOpenInfo info = UIWidgetOpenInfo.Create(widgetId, widgetType, owner, parentTrans, callback, userData);
            //     G.Asset.Load(GetUIWidgetRealAssetPath(widgetType), typeof(GameObject), LoadWidgetAssetCallback, info);
            //     JLog.Debug($"CreateWidget {widgetType} return null id {info.Id}");
            //     return widgetId;
            // }
            // else
            // {
            //     UIWidget widget = (UIWidget)poolObj.Target;
            //     OpenWidgetImpl(widgetId, widget, owner, false, callback, userData);
            //     JLog.Debug($"CreateWidget {widgetType} return id {widget.Id}");
            //     return widget.Id;
            // }
        }

        public void DestroyWidget(int widgetId)
        {
            if (_widgets.TryGetValue(widgetId, out UIWidget widget))
            {
                CloseWidgetImpl(widget);
            }
        }

        public T GetWidget<T>(int widgetId) where T : UIWidget
        {
            if (_widgets.TryGetValue(widgetId, out UIWidget widget))
                return (T)widget;
            return null;
        }

        private void LoadWidgetAssetCallback(AssetHandleWrap wrap)
        {
            UIWidgetOpenInfo widgetOpenInfo = (UIWidgetOpenInfo)wrap.UserData;
            if (widgetOpenInfo == null)
                throw new Exception("Open UI widget info is invalid.");
            if (wrap.IsSuccess)
            {
                UIWidget widget = GameObject.Instantiate((GameObject)wrap.Asset, widgetOpenInfo.ParentTrans)
                                            .GetComponent<UIWidget>();
                OpenWidgetImpl(widgetOpenInfo.Id, widget, widgetOpenInfo.Owner, widgetOpenInfo.ParentTrans, true, widgetOpenInfo.Callback, widgetOpenInfo.UserData);
                widgetOpenInfo.Dispose();
                JLog.Error($"LoadWidgetAssetCallback {widgetOpenInfo.Id}");
            }
            else
            {
                widgetOpenInfo.Dispose();
                string appendErrorMessage = Utils.Text.Format("Load UI widget failure, asset name '{0}'", wrap.AssetPath);
                throw new Exception(appendErrorMessage);
            }
        }

        private void TickWidget(float dt)
        {
            // while (_widgetRecycleQueue.Count > 0)
            // {
            //     UIWidget widget = _widgetRecycleQueue.Dequeue();
            //     widget.OnRecycle();
            //     _widgetPool.Unspawn(widget);
            // }

            foreach (var (_, w) in _widgets)
            {
                w.Tick(dt);
            }
        }

        private void LateTickWidget()
        {
            foreach (var (_, w) in _widgets)
            {
                w.LateTick();
            }
        }

        private void OpenWidgetImpl(int id, UIWidget widget, IWidgetOwner owner, Transform parentTrans, bool isNew, Action<UIWidget> callback, object userData)
        {
            if (isNew)
                widget.OnInit();
            _widgets.Add(id, widget);
            widget.SetId(id);
            widget.SetOwner(owner);
            widget.OnOpen(userData);
            widget.gameObject.SetActive(true);
            widget.transform.SetParent(parentTrans);
            widget.OnShow();
            callback?.Invoke(widget);
        }

        private void CloseWidgetImpl(UIWidget widget)
        {
            if (widget.IsVisible)
                widget.OnHide();
            _widgets.Remove(widget.Id);
            ReturnWidgetToPool(widget);
            // _widgetRecycleQueue.Enqueue(widget);
            widget.OnClose();
            widget.transform.SetParent(_widgetPoolRoot);
            widget.SetId(0);
            widget.SetOwner(null);
        }

        #region Widget Pool

        private UIWidget RentWidgetFromPool(UIWidgetId widgetType)
        {
            if (TryRentWidgetFromPool(widgetType, out UIWidget widget))
            {
                return widget;
            }
            return null;
        }

        private void ReturnWidgetToPool(UIWidget widget)
        {
            if (!_widgetPool.TryGetValue(widget.TypeId, out List<UIWidget> pool))
            {
                pool = new List<UIWidget>(8);
                _widgetPool.Add(widget.TypeId, pool);
            }
            pool.Add(widget);
        }

        private bool TryRentWidgetFromPool(UIWidgetId widgetType, out UIWidget outWidget)
        {
            outWidget = null;
            if (_widgetPool.TryGetValue(widgetType, out List<UIWidget> pool))
            {
                if (pool.Count > 0)
                {
                    outWidget = pool[^1];
                    pool.RemoveAt(pool.Count - 1);
                    return true;
                }
            }
            return false;
        }

        #endregion
    }

}