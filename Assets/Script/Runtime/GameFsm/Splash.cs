using Jam.Core;

namespace Jam.Runtime.GameFsm
{

    public class Splash : Fsm.State
    {
        public override void OnEnter(Fsm.State fromState)
        {
            JLog.Info("Enter Splash");
            // play some video
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
            ChangeState<ResourcePipeline>();
        }
    }

}