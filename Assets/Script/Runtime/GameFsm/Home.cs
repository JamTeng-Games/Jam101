using J.Core;
using J.Runtime.Event;

namespace J.Runtime.GameFsm
{
    public class Home : FsmState<Game>
    {
        public override void OnEnter(FsmState<Game> fromState)
        {
            EventMgr.Subscribe(GlobalEventId.EnterCombat, OnEnterCombat);
        }

        public override void OnExit()
        {
            EventMgr.Unsubscribe(GlobalEventId.EnterCombat, OnEnterCombat);
        }

        public override void OnTick(float dt)
        {
        }

        private void OnEnterCombat()
        {
            _fsm.ChangeState<Combat>();
        }
    }
}