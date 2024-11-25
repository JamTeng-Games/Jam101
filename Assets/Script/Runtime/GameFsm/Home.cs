using Jam.Core;
using Jam.Runtime.Event;

namespace Jam.Runtime.GameFsm
{
    public class Home : Fsm.State
    {
        public override void OnEnter(Fsm.State fromState)
        {
            G.Event.Subscribe(GlobalEventId.EnterCombat, OnEnterCombat);
        }

        public override void OnExit()
        {
            G.Event.Unsubscribe(GlobalEventId.EnterCombat, OnEnterCombat);
        }

        public override void OnTick(float dt)
        {
        }

        private void OnEnterCombat()
        {
            ChangeState<Combat>();
        }
    }
}