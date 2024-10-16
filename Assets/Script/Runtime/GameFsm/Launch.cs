using J.Core;

namespace J.Runtime.GameFsm
{
    public class Launch : FsmState<Game>
    {
        public override void OnEnter(FsmState<Game> fromState)
        {
            // Load config or something
            // Auto update or something

            _fsm.ChangeState<Opening>();
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
        }
    }
}