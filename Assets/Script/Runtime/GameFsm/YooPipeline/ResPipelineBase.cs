using Jam.Core;
using Jam.Runtime.Event;

namespace Jam.Runtime.GameFsm
{

    public class ResPipelineBase : Fsm.State
    {
        public override void OnEnter(Fsm.State fromState)
        {
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
        }

        public void SetProgress(float progress)
        {
            G.Event.Send(GlobalEventId.ResPipelineProgressUpdate, progress);
        }
    }

}