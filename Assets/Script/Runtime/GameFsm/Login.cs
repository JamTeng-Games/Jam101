using cfg;
using Jam.Core;
using Jam.Arena;
using Jam.Runtime.Event;

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
            
            G.UI.Open(UIPanelId.Home);
        }

        public override void OnExit()
        {
            G.Event.Unsubscribe(GlobalEventId.LoginSuccess, OnLoginSuccess);
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