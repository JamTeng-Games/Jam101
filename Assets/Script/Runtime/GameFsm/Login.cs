using J.Core;
using J.Runtime.Event;
using J.Runtime.UI;

namespace J.Runtime.GameFsm
{
    public class Login : FsmState<Game>
    {
        public override void OnEnter(FsmState<Game> fromState)
        {
            Game.UIMgr.Show<LoginPanel>();
            EventMgr.Subscribe(GlobalEventId.LoginSuccess, OnLoginSuccess);
        }

        public override void OnExit()
        {
            EventMgr.Unsubscribe(GlobalEventId.LoginSuccess, OnLoginSuccess);
        }

        public override void OnTick(float dt)
        {
        }

        private void OnLoginSuccess()
        {
            _fsm.ChangeState<Home>();
        }
    }
}