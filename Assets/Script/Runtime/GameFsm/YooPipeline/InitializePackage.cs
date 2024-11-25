using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Jam.Core;
using Jam.Runtime.Event;
using UnityEditor;
using UnityEngine;
using YooAsset;

namespace Jam.Runtime.GameFsm
{

    public class InitializePackage : ResPipelineBase
    {
        public override void OnEnter(Fsm.State fromState)
        {
            JLog.Info("初始化资源包");
            G.Event.Send(GlobalEventId.StartInitializePackage);
            SetProgress(0.1f);
            InitPackage().Forget();
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
        }

        private async UniTaskVoid InitPackage()
        {
            Asset.PlayMode playMode = (Asset.PlayMode)_fsm.Data.GetInt("PlayMode");
            var packageName = _fsm.Data.GetString("PackageName");
            var buildPipeline = _fsm.Data.GetString("BuildPipeline");

            // 创建资源包裹类
            var package = YooAssets.TryGetPackage(packageName);
            if (package == null)
                package = YooAssets.CreatePackage(packageName);

            // 编辑器下的模拟模式
            InitializationOperation initializationOperation = null;
            if (playMode == Asset.PlayMode.EditorSimulateMode)
            {
                var simulateBuildResult = EditorSimulateModeHelper.SimulateBuild(buildPipeline, packageName);
                var createParameters = new EditorSimulateModeParameters();
                createParameters.EditorFileSystemParameters =
                    FileSystemParameters.CreateDefaultEditorFileSystemParameters(simulateBuildResult);
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // 单机运行模式
            if (playMode == Asset.PlayMode.OfflinePlayMode)
            {
                var createParameters = new OfflinePlayModeParameters();
                createParameters.BuildinFileSystemParameters =
                    FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // 联机运行模式
            if (playMode == Asset.PlayMode.HostPlayMode)
            {
                string defaultHostServer = GetHostServerURL();
                string fallbackHostServer = GetHostServerURL();
                IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                var createParameters = new HostPlayModeParameters();
                createParameters.BuildinFileSystemParameters =
                    FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
                createParameters.CacheFileSystemParameters =
                    FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // WebGL运行模式
            if (playMode == Asset.PlayMode.WebPlayMode)
            {
                var createParameters = new WebPlayModeParameters();
                createParameters.WebFileSystemParameters = FileSystemParameters.CreateDefaultWebFileSystemParameters();
                initializationOperation = package.InitializeAsync(createParameters);
            }

            await initializationOperation.ToUniTask();

            // 如果初始化失败弹出提示界面
            if (initializationOperation.Status != EOperationStatus.Succeed)
            {
                JLog.Warning($"{initializationOperation.Error}");
                G.Event.Send(GlobalEventId.InitResourceFailed);
            }
            else
            {
                ChangeState<UpdatePackageVersion>();
            }
        }

        /// <summary>
        /// 获取资源服务器地址
        /// </summary>
        private string GetHostServerURL()
        {
            //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
            string hostServerIP = "http://127.0.0.1";
            string appVersion = "v1.0";

#if UNITY_EDITOR
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
                return $"{hostServerIP}/CDN/Android/{appVersion}";
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
                return $"{hostServerIP}/CDN/IPhone/{appVersion}";
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebGL)
                return $"{hostServerIP}/CDN/WebGL/{appVersion}";
            else
                return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
        if (Application.platform == RuntimePlatform.Android)
            return $"{hostServerIP}/CDN/Android/{appVersion}";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            return $"{hostServerIP}/CDN/IPhone/{appVersion}";
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
            return $"{hostServerIP}/CDN/WebGL/{appVersion}";
        else
            return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
        }

        /// <summary>
        /// 远端资源地址查询服务类
        /// </summary>
        private class RemoteServices : IRemoteServices
        {
            private readonly string _defaultHostServer;
            private readonly string _fallbackHostServer;

            public RemoteServices(string defaultHostServer, string fallbackHostServer)
            {
                _defaultHostServer = defaultHostServer;
                _fallbackHostServer = fallbackHostServer;
            }

            string IRemoteServices.GetRemoteMainURL(string fileName)
            {
                return $"{_defaultHostServer}/{fileName}";
            }

            string IRemoteServices.GetRemoteFallbackURL(string fileName)
            {
                return $"{_fallbackHostServer}/{fileName}";
            }
        }

        public class BundleStream : FileStream
        {
            public const byte KEY = 64;

            public BundleStream(string path, FileMode mode, FileAccess access, FileShare share) : base(
                path, mode, access, share)
            {
            }

            public BundleStream(string path, FileMode mode) : base(path, mode)
            {
            }

            public override int Read(byte[] array, int offset, int count)
            {
                var index = base.Read(array, offset, count);
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] ^= KEY;
                }
                return index;
            }
        }

        /// <summary>
        /// 资源文件流加载解密类
        /// </summary>
        private class FileStreamDecryption : IDecryptionServices
        {
            /// <summary>
            /// 同步方式获取解密的资源包对象
            /// 注意：加载流对象在资源包对象释放的时候会自动释放
            /// </summary>
            AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
            {
                BundleStream bundleStream =
                    new BundleStream(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                managedStream = bundleStream;
                return AssetBundle.LoadFromStream(bundleStream, fileInfo.FileLoadCRC, GetManagedReadBufferSize());
            }

            /// <summary>
            /// 异步方式获取解密的资源包对象
            /// 注意：加载流对象在资源包对象释放的时候会自动释放
            /// </summary>
            AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo,
                                                                              out Stream managedStream)
            {
                BundleStream bundleStream =
                    new BundleStream(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                managedStream = bundleStream;
                return AssetBundle.LoadFromStreamAsync(bundleStream, fileInfo.FileLoadCRC, GetManagedReadBufferSize());
            }

            /// <summary>
            /// 获取解密的字节数据
            /// </summary>
            byte[] IDecryptionServices.ReadFileData(DecryptFileInfo fileInfo)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// 获取解密的文本数据
            /// </summary>
            string IDecryptionServices.ReadFileText(DecryptFileInfo fileInfo)
            {
                throw new NotImplementedException();
            }

            private static uint GetManagedReadBufferSize()
            {
                return 1024;
            }
        }

        /// <summary>
        /// 资源文件偏移加载解密类
        /// </summary>
        private class FileOffsetDecryption : IDecryptionServices
        {
            /// <summary>
            /// 同步方式获取解密的资源包对象
            /// 注意：加载流对象在资源包对象释放的时候会自动释放
            /// </summary>
            AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
            {
                managedStream = null;
                return AssetBundle.LoadFromFile(fileInfo.FileLoadPath, fileInfo.FileLoadCRC, GetFileOffset());
            }

            /// <summary>
            /// 异步方式获取解密的资源包对象
            /// 注意：加载流对象在资源包对象释放的时候会自动释放
            /// </summary>
            AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo,
                                                                              out Stream managedStream)
            {
                managedStream = null;
                return AssetBundle.LoadFromFileAsync(fileInfo.FileLoadPath, fileInfo.FileLoadCRC, GetFileOffset());
            }

            /// <summary>
            /// 获取解密的字节数据
            /// </summary>
            byte[] IDecryptionServices.ReadFileData(DecryptFileInfo fileInfo)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// 获取解密的文本数据
            /// </summary>
            string IDecryptionServices.ReadFileText(DecryptFileInfo fileInfo)
            {
                throw new NotImplementedException();
            }

            private static ulong GetFileOffset()
            {
                return 32;
            }
        }
    }

}