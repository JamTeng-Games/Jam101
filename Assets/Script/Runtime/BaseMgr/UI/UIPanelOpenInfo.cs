using System;
using cfg;
using Jam.Core;

namespace Jam.Runtime.UI_
{

    public sealed class UIPanelOpenInfo : IReference
    {
        private UIPanelId _panelId;
        private UIShowMode _showMode;
        private UILevel _level;
        private Action<UIPanel> _callback;
        private object _userData;

        public UIPanelId PanelId => _panelId;
        public UIShowMode ShowMode => _showMode;
        public UILevel Level => _level;
        public Action<UIPanel> Callback => _callback;
        public object UserData => _userData;

        public static UIPanelOpenInfo Create(UIPanelId panelId,
                                             UIShowMode showMode,
                                             UILevel level,
                                             Action<UIPanel> callback,
                                             object userData)
        {
            UIPanelOpenInfo info = ReferencePool.Get<UIPanelOpenInfo>();
            info._panelId = panelId;
            info._showMode = showMode;
            info._level = level;
            info._callback = callback;
            info._userData = userData;
            return info;
        }

        public void Dispose()
        {
            ReferencePool.Release(this);
        }

        public void UpdateInfo(UIShowMode newMode, UILevel newLevel, Action<UIPanel> callback, object userData)
        {
            if (newMode != UIShowMode.None)
                _showMode = newMode;
            if (newLevel != UILevel.None)
                _level = newLevel;
            if (callback != null)
                _callback += callback;
            if (userData != null)
                _userData = userData;
        }

        void IReference.Clean()
        {
            _panelId = default;
            _showMode = default;
            _level = default;
            _callback = null;
            _userData = default;
        }
    }

}