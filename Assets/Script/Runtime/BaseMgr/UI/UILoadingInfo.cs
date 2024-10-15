using System;

namespace J.Runtime.UI
{
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
        private UIPanelConfig _config;

        public Action<T> callback;
        public UIShowMode ShowMode => _config.ShowMode;
        public UILevel Level => _config.Level;

        public LoadingInfoPanel(Action<T> callback, UIShowMode showMode, UILevel level) : base(typeof(T))
        {
            this.callback = callback;
            UIPanelConfig originCfg = UIMgr.Config.Get<T>();
            _config = originCfg.New(showMode, level);
        }

        public void UpdateConfig(UIShowMode showMode, UILevel level)
        {
            _config = _config.New(showMode, level);
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
}