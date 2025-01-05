using cfg;
using Jam.Core;
using Jam.Arena;
using Jam.Runtime.Event;
using Jam.Runtime.Net_;
using Jam.Runtime.UI_;
using UnityEngine;

namespace Jam.Runtime.GameFsm
{

    public class Login : Fsm.State
    {
        public override void OnEnter(Fsm.State fromState)
        {
            JLog.Info("Enter Login");
            G.Event.Subscribe(GlobalEventId.LoginSuccess, OnLoginSuccess);
            G.Login.TryLogin();
        }

        public override void OnExit()
        {
            JLog.Info("Login.OnExit");
            StartupPanel.CloseSelf();
            G.Event.Unsubscribe(GlobalEventId.LoginSuccess, OnLoginSuccess);
        }

        public override void OnTick(float dt)
        {
        }

        private void OnLoginSuccess()
        {
            ChangeState<Home>();
        }
    }

}