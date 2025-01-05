using Jam.Core;

namespace Jam.Runtime.UI_
{

    public partial class JoystickWidget
    {
        public override void OnInit()
        {
            JLog.Info("JoystickWidget OnInit");
        }

        public override void OnOpen(object userData)
        {
            JLog.Info("JoystickWidget OnOpen");
        }

        public override void OnClose()
        {
            JLog.Info("JoystickWidget OnClose");
        }

        protected override void OnTick(float dt)
        {
            JLog.Info("JoystickWidget Tick");
        }
    }

}