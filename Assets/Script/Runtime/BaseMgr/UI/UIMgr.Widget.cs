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
        private Queue<UIWidget> _widgetRecycleQueue;

        // [SerializeField] private float _widgetPoolAutoReleaseInterval = 60f;
        [SerializeField] private int _widgetPoolCapacity = 16;
        [SerializeField] private float _widgetPoolExpireTime = 60f;

        private int _widgetGenId = 0;
        private IObjectPool<UIWidgetObject> _widgetPool;

        private Dictionary<int, UIWidget> _widgets;

        private void InitWidget()
        {
            _widgetPool =
                G.ObjectPool.CreateSingleSpawnObjectPool<UIWidgetObject>("UIWidgetPool", _widgetPoolCapacity, _widgetPoolExpireTime);
        }

        public int CreateWidget(UIWidgetId widgetType,
                                IWidgetOwner owner,
                                Transform parentTrans,
                                Action<UIWidget> callback,
                                object userData = null)
        {
            int widgetId = ++_widgetGenId;
            // 加载资源
            UIWidgetObject poolObj = _widgetPool.Spawn(GetUIWidgetRealAssetPath(widgetType) + widgetId.ToString());
            if (poolObj == null)
            {
                UIWidgetOpenInfo info = UIWidgetOpenInfo.Create(widgetId, widgetType, owner, parentTrans, callback, userData);
                G.Asset.Load(GetUIWidgetRealAssetPath(widgetType), typeof(GameObject), LoadWidgetAssetCallback, info);
                JLog.Debug($"CreateWidget {widgetType} return null id {info.Id}");
                return widgetId;
            }
            else
            {
                UIWidget widget = (UIWidget)poolObj.Target;
                OpenWidgetImpl(widgetId, widget, owner, false, callback, userData);
                JLog.Debug($"CreateWidget {widgetType} return id {widget.Id}");
                return widget.Id;
            }
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
                UIWidgetObject poolObj = UIWidgetObject.Create(wrap.AssetPath, wrap.Id, widget);
                _widgetPool.Register(poolObj, true);
                OpenWidgetImpl(widgetOpenInfo.Id, widget, widgetOpenInfo.Owner, true, widgetOpenInfo.Callback, widgetOpenInfo.UserData);
                widgetOpenInfo.Dispose();
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
            while (_widgetRecycleQueue.Count > 0)
            {
                UIWidget widget = _widgetRecycleQueue.Dequeue();
                widget.OnRecycle();
                _widgetPool.Unspawn(widget);
            }

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

        private void OpenWidgetImpl(int id, UIWidget widget, IWidgetOwner owner, bool isNew, Action<UIWidget> callback, object userData)
        {
            if (isNew)
                widget.OnInit();
            _widgets.Add(id, widget);
            widget.SetId(id);
            widget.SetOwner(owner);
            widget.OnOpen(userData);
            widget.gameObject.SetActive(true);
            widget.OnShow();
            callback?.Invoke(widget);
        }

        private void CloseWidgetImpl(UIWidget widget)
        {
            if (widget.IsVisible)
                widget.OnHide();
            _widgets.Remove(widget.Id);
            _widgetRecycleQueue.Enqueue(widget);
            widget.OnClose();
            widget.gameObject.SetActive(false);
            widget.SetId(0);
            widget.SetOwner(null);
        }
    }

}