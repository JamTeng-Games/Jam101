using Cysharp.Threading.Tasks;
using Jam.Core;
using Jam.Runtime.Event;
using UnityEngine;
using YooAsset;

namespace Jam.Runtime.GameFsm
{

    public class CreatePackageDownloader : ResPipelineBase
    {
        public override void OnEnter(Fsm.State fromState)
        {
            JLog.Info("创建补丁下载器");
            SetProgress(0.8f);
            CreateDownloader().Forget();
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
        }

        private async UniTaskVoid CreateDownloader()
        {
            await UniTask.WaitForSeconds(0.1f);

            var packageName = _fsm.Data.GetString("PackageName");
            var package = YooAssets.GetPackage(packageName);
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            _fsm.Data.SetObject("Downloader", downloader);

            if (downloader.TotalDownloadCount == 0)
            {
                JLog.Info("Not found any download files !");
                ChangeState<UpdateResDone>();
            }
            else
            {
                // 发现新更新文件后，挂起流程系统
                // 注意：开发者需要在下载前检测磁盘空间不足
                int totalDownloadCount = downloader.TotalDownloadCount;
                long totalDownloadBytes = downloader.TotalDownloadBytes;

                float sizeMB = totalDownloadBytes / 1048576f;
                sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
                string totalSizeMB = sizeMB.ToString("f1");

                G.Event.Send(GlobalEventId.PrepareDownloadPackageFiles, totalSizeMB);
                SetProgress(0f);
                ChangeState<DownloadPackageFiles>();
            }
        }
    }

}