using Cysharp.Threading.Tasks;
using Jam.Core;
using Jam.Runtime.Event;
using YooAsset;

namespace Jam.Runtime.GameFsm
{

    public class UpdatePackageManifest : ResPipelineBase
    {
        public override void OnEnter(Fsm.State fromState)
        {
            JLog.Info("更新资源清单");
            G.Event.Send(GlobalEventId.StartUpdatePackageManifest);
            SetProgress(0.6f);
            UpdateManifest().Forget();
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
        }

        private async UniTaskVoid UpdateManifest()
        {
            await UniTask.WaitForSeconds(0.5f);

            var packageName = _fsm.Data.GetString("PackageName");
            var packageVersion = _fsm.Data.GetString("PackageVersion");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.UpdatePackageManifestAsync(packageVersion);
            await operation.ToUniTask();

            if (operation.Status != EOperationStatus.Succeed)
            {
                JLog.Warning(operation.Error);
                G.Event.Send(GlobalEventId.PatchManifestUpdateFailed);
                return;
            }
            else
            {
                ChangeState<CreatePackageDownloader>();
            }
        }
    }

}