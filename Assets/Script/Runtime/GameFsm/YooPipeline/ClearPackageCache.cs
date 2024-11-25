using Jam.Core;
using YooAsset;

namespace Jam.Runtime.GameFsm
{

    public class ClearPackageCache : ResPipelineBase
    {
        public override void OnEnter(Fsm.State fromState)
        {
            JLog.Info("清理未使用的缓存文件");
            var packageName = _fsm.Data.GetString("PackageName");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.ClearUnusedBundleFilesAsync();
            operation.Completed += Operation_Completed;
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
        }

        private void Operation_Completed(YooAsset.AsyncOperationBase obj)
        {
            ChangeState<UpdateResDone>();
        }
    }

}