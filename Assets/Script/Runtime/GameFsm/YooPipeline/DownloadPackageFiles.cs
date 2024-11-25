using Cysharp.Threading.Tasks;
using Jam.Core;
using Jam.Runtime.Event;
using YooAsset;

namespace Jam.Runtime.GameFsm
{

    public struct DownloadProgress
    {
        public int TotalCount;
        public int CurrentCount;
        public long TotalBytes;
        public long CurrentBytes;
    }

    public class DownloadPackageFiles : ResPipelineBase
    {
        public override void OnEnter(Fsm.State fromState)
        {
            G.Event.Subscribe(GlobalEventId.AgreeDownloadPackageFiles, OnUserAgreeToDownloadFile);
        }

        public override void OnExit()
        {
            G.Event.Unsubscribe(GlobalEventId.AgreeDownloadPackageFiles, OnUserAgreeToDownloadFile);
        }

        public override void OnTick(float dt)
        {
        }

        private void OnUserAgreeToDownloadFile()
        {
            BeginDownload().Forget();
        }

        private async UniTaskVoid BeginDownload()
        {
            JLog.Info("开始下载补丁文件");
            G.Event.Send(GlobalEventId.StartDownloadPackageFiles);
            var downloader = (ResourceDownloaderOperation)_fsm.Data.GetObject("Downloader");
            downloader.OnDownloadErrorCallback = OnDownloadError;
            downloader.OnDownloadProgressCallback = OnDownloadProgress;
            downloader.BeginDownload();
            await downloader.ToUniTask();

            // 检测下载结果
            if (downloader.Status != EOperationStatus.Succeed)
            {
                JLog.Error("Download error");
                return;
            }

            ChangeState<DownloadPackageOver>();
        }

        private void OnDownloadProgress(int totalDownloadCount,
                                        int currentDownloadCount,
                                        long totalDownloadBytes,
                                        long currentDownloadBytes)
        {
            // SetProgress(currentDownloadCount / (float)totalDownloadCount);
            G.Event.Send(GlobalEventId.DownloadPackageFileProgress,
                         new DownloadProgress
                         {
                             TotalCount = totalDownloadCount,
                             CurrentCount = currentDownloadCount,
                             TotalBytes = totalDownloadBytes,
                             CurrentBytes = currentDownloadBytes
                         });
            JLog.Info(
                $"下载进度: {currentDownloadCount}/{totalDownloadCount}, {currentDownloadBytes}/{totalDownloadBytes}");
        }

        private void OnDownloadError(string fileName, string error)
        {
            JLog.Error($"下载错误: file: {fileName}, error: {error}");
        }
    }

}