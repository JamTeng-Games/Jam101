using Jam.Core;
using Jam.Runtime.Event;

namespace Jam.Runtime.GameFsm
{

    public class UpdateResDone : ResPipelineBase
    {
        public override void OnEnter(Fsm.State fromState)
        {
            G.Event.Send(GlobalEventId.UpdateResDone);
            SetProgress(1f);
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
        }
    }

}