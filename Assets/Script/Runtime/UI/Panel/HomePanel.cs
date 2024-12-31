using cfg;
using Jam.Core;
using Jam.Runtime.Event;

namespace Jam.Runtime.UI_
{

    public partial class HomePanel
    {
        private int _widgetId = 0;

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_battle.onClick.AddListener(OnClickBattle);
            _btn_settings.onClick.AddListener(OnClickSettings);
        }

        public override void OnClose()
        {
            _btn_battle.onClick.RemoveListener(OnClickBattle);
            _btn_settings.onClick.RemoveListener(OnClickSettings);

            this.ClearWidgets();
        }

        protected override void OnTick(float dt)
        {
        }

        private void OnClickBattle()
        {
            G.Event.Send(GlobalEventId.EnterCombat);
        }

        private void OnClickSettings()
        {
            if (_widgetId == 0)
            {
                _widgetId = this.AddWidget(UIWidgetId.Joystick, _slider_exp.transform, w =>
                {
                    JLog.Info("JoystickWidget added");
                });
            }
            else
            {
                this.RemoveWidget(_widgetId);
                _widgetId = 0;
            }
        }
    }

}