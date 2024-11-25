using Jam.Core;

namespace Jam.Runtime.GameFsm
{

    public class DownloadPackageOver : ResPipelineBase
    {
        public override void OnEnter(Fsm.State fromState)
        {
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
            ChangeState<ClearPackageCache>();
        }
    }

}