using System;
using Jam.Cfg;
using Jam.Core;
using UnityEngine;

namespace Jam.Runtime.UI_
{

    public class UIWidgetOpenInfo : IReference
    {
        private int _id;
        private UIWidgetId _widgetTypeId;
        private IWidgetOwner _owner;
        private Transform _parentTrans;
        private Action<UIWidget> _callback;
        private object _userData;

        public int Id => _id;
        public UIWidgetId WidgetTypeId => _widgetTypeId;
        public IWidgetOwner Owner => _owner;
        public Transform ParentTrans => _parentTrans;
        public Action<UIWidget> Callback => _callback;
        public object UserData => _userData;

        public static UIWidgetOpenInfo Create(int id,
                                              UIWidgetId widgetTypeId,
                                              IWidgetOwner owner,
                                              Transform parentTrans,
                                              Action<UIWidget> callback,
                                              object userData)
        {
            UIWidgetOpenInfo info = new UIWidgetOpenInfo();
            info._id = id;
            info._widgetTypeId = widgetTypeId;
            info._owner = owner;
            info._parentTrans = parentTrans;
            info._callback = callback;
            info._userData = userData;
            return info;
        }

        public void Dispose()
        {
            ReferencePool.Release(this);
        }

        public void UpdateInfo(IWidgetOwner owner,
                               Transform parentTrans,
                               Action<UIWidget> callback,
                               object userData)
        {
            if (owner != null)
                _owner = owner;
            if (parentTrans != null)
                _parentTrans = parentTrans;
            if (callback != null)
                _callback += callback;
            if (userData != null)
                _userData = userData;
        }

        void IReference.Clean()
        {
            _id = 0;
            _widgetTypeId = default;
            _owner = null;
            _parentTrans = null;
            _callback = null;
            _userData = default;
        }
    }

}