using System;
using System.Collections.Generic;

namespace J.Runtime.UI
{
    public struct UIPanelConfig
    {
        private readonly Type _type;
        private readonly UIShowMode _showMode;
        private readonly UILevel _level;

        public Type Type => _type;
        public UIShowMode ShowMode => _showMode;
        public UILevel Level => _level;

        public UIPanelConfig(Type type, UIShowMode showMode, UILevel level)
        {
            _type = type;
            _showMode = showMode;
            _level = level;
        }

        public UIPanelConfig New(UIShowMode showMode, UILevel level)
        {
            UIShowMode newShowMode = showMode != UIShowMode.None ? showMode : _showMode;
            UILevel newLevel = level != UILevel.None ? level : _level;
            return new UIPanelConfig(_type, newShowMode, newLevel);
        }
    }

    public class UIConfig
    {
        private UIPanelConfig _defaultPanelConfig;
        private Dictionary<Type, UIPanelConfig> _panelConfigs;

        public UIConfig()
        {
            _defaultPanelConfig = new UIPanelConfig(null, UIShowMode.Cover, UILevel.Mid);
            _panelConfigs = new Dictionary<Type, UIPanelConfig>();
            LoadConfig();
        }

        public UIPanelConfig Get<T>() where T : UIPanel
        {
            return _panelConfigs.GetValueOrDefault(typeof(T), _defaultPanelConfig);
        }

        private void LoadConfig()
        {
            AddPanelConfig<LoginPanel>(UIShowMode.Cover, UILevel.Mid);
            AddPanelConfig<TestPanel>(UIShowMode.Push, UILevel.Mid);
            AddPanelConfig<TestBotPanel>(UIShowMode.Replace, UILevel.Mid);
        }

        private void AddPanelConfig<T>(UIShowMode showMode, UILevel level) where T : UIPanel
        {
            _panelConfigs.Add(typeof(T), new UIPanelConfig(typeof(T), showMode, level));
        }
    }
}