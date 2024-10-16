using J.Core;

namespace J.Runtime.GameFsm
{
    public class Opening : FsmState<Game>
    {
        public override void OnEnter(FsmState<Game> fromState)
        {
            // play some video
            _fsm.ChangeState<Login>();
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
        }
    }
}