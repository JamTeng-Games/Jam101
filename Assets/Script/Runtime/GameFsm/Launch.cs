using Jam.Core;

namespace Jam.Runtime.GameFsm
{

    public class Launch : Fsm.State
    {
        public override void OnEnter(Fsm.State fromState)
        {
            // Load config or something
            // Auto update or something

            JLog.Info("Enter Launch State");
            ChangeState<Splash>();
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
        }
    }

}