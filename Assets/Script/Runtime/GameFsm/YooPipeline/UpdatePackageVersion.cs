using Cysharp.Threading.Tasks;
using Jam.Core;
using Jam.Runtime.Event;
using YooAsset;

namespace Jam.Runtime.GameFsm
{

    public class UpdatePackageVersion : ResPipelineBase
    {
        public override void OnEnter(Fsm.State fromState)
        {
            JLog.Info("获取最新的资源版本!");
            G.Event.Send(GlobalEventId.StartUpdatePackageVersion);
            SetProgress(0.3f);
            UpdatePackageVersions().Forget();
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
        }

        private async UniTaskVoid UpdatePackageVersions()
        {
            await UniTask.WaitForSeconds(0.5f);

            var packageName = _fsm.Data.GetString("PackageName");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.RequestPackageVersionAsync();
            await operation.ToUniTask();

            if (operation.Status != EOperationStatus.Succeed)
            {
                JLog.Warning(operation.Error);
                G.Event.Send(GlobalEventId.PackageVersionUpdateFailed);
            }
            else
            {
                JLog.Info($"Request package version : {operation.PackageVersion}");
                _fsm.Data.SetString("PackageVersion", operation.PackageVersion);
                ChangeState<UpdatePackageManifest>();
            }
        }
    }

}