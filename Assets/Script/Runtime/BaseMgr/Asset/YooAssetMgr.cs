using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jam.Core;
using Jam.Runtime.Event;
using UnityEngine;
using YooAsset;
using Object = UnityEngine.Object;

namespace Jam.Runtime.Asset
{

    public class YooAssetMgr : IAssetMgr
    {
        private static int _handleId = 0;
        private Dictionary<int, AssetHandle> _handles;
        private Dictionary<AssetHandle, int> _handleIds;
        private Dictionary<int, AssetHandleWrap> _handleWraps;
        private ResourcePackage _mainPackage;

        public YooAssetMgr()
        {
            _handles = new Dictionary<int, AssetHandle>();
            _handleIds = new Dictionary<AssetHandle, int>();
            _handleWraps = new Dictionary<int, AssetHandleWrap>();

            YooAssets.Initialize();
        }

        public void Init()
        {
            G.Event.Subscribe(GlobalEventId.ResPipelineDone, OnResPipelineDone);
        }

        private void OnResPipelineDone()
        {
            _mainPackage = YooAssets.GetPackage("DefaultPackage");
            YooAssets.SetDefaultPackage(_mainPackage);
        }

        public void Shutdown(bool isAppQuit)
        {
        }

        // UniTask load async
        public async UniTask<AssetHandleWrap> Load(string assetPath, Type objType)
        {
            AssetHandle handle = _mainPackage.LoadAssetAsync(assetPath, objType);
            AssetHandleWrap wrap = CreateHandleWrap(handle, assetPath, null);
            await handle.ToUniTask();
            // Failed
            if (handle.Status == EOperationStatus.Failed)
            {
                RemoveHandleWrap(handle);
                return null;
            }
            // Success
            return wrap;
        }

        // Callback load async
        public void Load(string assetPath, Type objType, Action<AssetHandleWrap> callback, object userData)
        {
            AssetHandle handle = _mainPackage.LoadAssetAsync(assetPath, objType);
            CreateHandleWrap(handle, assetPath, userData);
            handle.Completed += (handleSelf) =>
            {
                // Failed
                if (handle.Status == EOperationStatus.Failed)
                {
                    callback?.Invoke(GetHandleWrap(handleSelf));
                    RemoveHandleWrap(handleSelf);
                }
                // Success
                else
                {
                    callback?.Invoke(GetHandleWrap(handleSelf));
                }
            };
        }

        public void Unload(int handleId)
        {
            RemoveHandleWrap(handleId);
        }

        /// Dont use!!!
        public Object SyncLoad(string assetPath, System.Type objType, out int handleId)
        {
            AssetHandle handle = _mainPackage.LoadAssetSync(assetPath, objType);
            AssetHandleWrap wrap = CreateHandleWrap(handle, assetPath, null);
            // Failed
            if (handle.Status == EOperationStatus.Failed)
            {
                handleId = 0;
                RemoveHandleWrap(handle);
                return null;
            }
            // Success
            handleId = wrap.Id;
            return handle.AssetObject;
        }

        public void UnloadUnused()
        {
            // Async
            _mainPackage.UnloadUnusedAssetsAsync();
        }

        public void ForceUnloadAll()
        {
            // Async
            _mainPackage.UnloadAllAssetsAsync();
        }

        // 尝试卸载指定的资源对象
        // 注意：如果该资源还在被使用，该方法会无效。
        public void TryUnloadUnused(string assetPath)
        {
            _mainPackage.TryUnloadUnusedAsset(assetPath);
        }

        // Helpers
        private int GetHandleId(AssetHandle handle)
        {
            return _handleIds.GetValueOrDefault(handle, 0);
        }

        private AssetHandle GetHandle(int handleId)
        {
            return _handles.GetValueOrDefault(handleId);
        }

        private AssetHandleWrap GetHandleWrap(int handleId)
        {
            return _handleWraps.GetValueOrDefault(handleId);
        }

        private AssetHandleWrap GetHandleWrap(AssetHandle handle)
        {
            int handleId = GetHandleId(handle);
            return GetHandleWrap(handleId);
        }

        private AssetHandleWrap CreateHandleWrap(AssetHandle handle, string assetPath, object userData)
        {
            int handleId = _handleId++;
            return AddHandleWrap(handleId, handle, assetPath, userData);
        }

        private AssetHandleWrap AddHandleWrap(int handleId, AssetHandle handle, string assetPath, object userData)
        {
            AssetHandleWrap wrap = AssetHandleWrap.Create(handleId, handle, assetPath, userData);
            _handles.Add(handleId, handle);
            _handleIds.Add(handle, handleId);
            _handleWraps.Add(handleId, wrap);
            return wrap;
        }

        private void RemoveHandleWrap(int handleId)
        {
            if (_handles.TryGetValue(handleId, out var handle))
            {
                RemoveHandleWrap(handleId, handle);
            }
        }

        private void RemoveHandleWrap(AssetHandle handle)
        {
            if (_handleIds.TryGetValue(handle, out var handleId))
            {
                RemoveHandleWrap(handleId, handle);
            }
        }

        private void RemoveHandleWrap(int handleId, AssetHandle handle)
        {
            _handles.Remove(handleId);
            _handleIds.Remove(handle);
            if (_handleWraps.Remove(handleId, out var wrap))
            {
                wrap.Dispose();
            }
        }
    }

    public class AssetHandleWrap : IReference
    {
        private int _id;
        private AssetHandle _handle;
        private object _userData;
        private string _assetPath;
        private Object _asset; // Editor 模式用, 没有AssetHandle

        public int Id => _id;
        public AssetHandle Handle => _handle;
        public object UserData => _userData;
        public Object Asset => _asset ?? _handle?.AssetObject;
        public bool IsSuccess => Asset != null;
        public string AssetPath => _assetPath;

        public static AssetHandleWrap Create(int id, AssetHandle handle, string assetPath, object userData)
        {
            AssetHandleWrap wrap = ReferencePool.Get<AssetHandleWrap>();
            wrap._id = id;
            wrap._handle = handle;
            wrap._userData = userData;
            wrap._assetPath = assetPath;
            return wrap;
        }

        public static AssetHandleWrap Create(int id, Object asset, string assetPath, object userData)
        {
            if (!Application.isEditor)
                return null;
            AssetHandleWrap wrap = ReferencePool.Get<AssetHandleWrap>();
            wrap._id = id;
            wrap._asset = asset;
            wrap._userData = userData;
            wrap._assetPath = assetPath;
            return wrap;
        }

        public void Dispose()
        {
            _handle?.Release();
            ReferencePool.Release(this);
        }

        public void Clean()
        {
            _id = 0;
            _handle = null;
            _userData = null;
            _assetPath = null;
            _asset = null;
        }
    }

    public enum PlayMode
    {
        /// <summary>
        /// 编辑器下的模拟模式
        /// </summary>
        EditorSimulateMode,

        /// <summary>
        /// 离线运行模式
        /// </summary>
        OfflinePlayMode,

        /// <summary>
        /// 联机运行模式
        /// </summary>
        HostPlayMode,

        /// <summary>
        /// WebGL运行模式
        /// </summary>
        WebPlayMode,
    }

    public enum DefaultBuildPipeline
    {
        /// <summary>
        /// 内置构建管线
        /// </summary>
        BuiltinBuildPipeline,

        /// <summary>
        /// 可编程构建管线
        /// </summary>
        ScriptableBuildPipeline,

        /// <summary>
        /// 原生文件构建管线
        /// </summary>
        RawFileBuildPipeline,
    }

}