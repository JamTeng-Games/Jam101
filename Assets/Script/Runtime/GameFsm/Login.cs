using cfg;
using Jam.Core;
using Jam.Arena;
using Jam.Runtime.Event;
using Jam.Runtime.UI_;

namespace Jam.Runtime.GameFsm
{

    public class Login : Fsm.State
    {
        public override void OnEnter(Fsm.State fromState)
        {
            JLog.Info("Enter Login");

            // _connect = new ConnectToPhoton();
            // _connect.ConnectToMaster();
            // ConnectToPhoton.Instance.ConnectToMaster();

            // foreach (var item in G.Cfg.TbItem.DataList)
            // {
            //     JLog.Info(item.ToString());
            // }

            // Game.UI.Open(UIPanelId.Login);
            // Game.Event.Subscribe(GlobalEventId.LoginSuccess, OnLoginSuccess);

            ChangeState<Home>();
        }

        public override void OnExit()
        {
            JLog.Info("Login.OnExit");
            StartupPanel.CloseSelf();
            // G.Event.Unsubscribe(GlobalEventId.LoginSuccess, OnLoginSuccess);
        }

        public override void OnTick(float dt)
        {
            // ConnectToPhoton.Instance.Tick(dt);
        }

        private void OnLoginSuccess()
        {
            ChangeState<Home>();
        }
    }

}