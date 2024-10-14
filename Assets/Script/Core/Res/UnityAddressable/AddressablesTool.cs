using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace J.Core
{
    public class AddressablesAsset : Asset
    {
        private AsyncOperationHandle _handle;

        public AsyncOperationHandle Handle => _handle;
        public override object Res => _handle.Result;
        public override StoreType StoreType => StoreType.Addressables;
        public override bool IsDone => _handle.IsDone;

        public AddressablesAsset(string name, AsyncOperationHandle handle, Type assetType) : base(name, assetType)
        {
            _handle = handle;
        }
    }

    public sealed class AddressablesTool
    {
        public AddressablesTool()
        {
        }

        // Sync load
        public T Load<T>(string name)
        {
            AsyncOperationHandle<T> handle;
            // already loaded before
            if (AssetPool.Instance.Get(name) is AddressablesAsset asset)
            {
                handle = asset.Handle.Convert<T>();
                if (handle.IsDone)
                {
                    return handle.Result;
                }
                else
                {
                    handle.WaitForCompletion();
                    return handle.Result;
                }
            }

            // has not loaded before
            handle = Addressables.LoadAssetAsync<T>(name);
            T result = handle.WaitForCompletion();
            AddressablesAsset newAsset = new AddressablesAsset(name, handle, typeof(T));
            AssetPool.Instance.Add(newAsset);
            return result;
        }

        #region UniTask async

        public UniTask<T> LoadFromAA<T>(string name)
        {
            AsyncOperationHandle<T> handle;
            // already loaded before
            if (AssetPool.Instance.Get(name) is AddressablesAsset asset)
            {
                handle = asset.Handle.Convert<T>();
                if (handle.IsDone)
                {
                    return UniTask.FromResult(handle.Result);
                }
                else
                {
                    var utcs = new UniTaskCompletionSource<T>();
                    handle.Completed += (obj) =>
                    {
                        if (obj.Status == AsyncOperationStatus.Succeeded)
                            utcs.TrySetResult(obj.Result);
                        else
                            utcs.TrySetException(new Exception($"load asset failed: {name}"));
                    };
                    return utcs.Task;
                }
            }

            // has not loaded before
            handle = Addressables.LoadAssetAsync<T>(name);
            AddressablesAsset newAsset = new AddressablesAsset(name, handle, typeof(T));
            AssetPool.Instance.Add(newAsset);
            return handle.ToUniTask<T>();
        }

        #endregion UniTask async

        #region Callback async

        public AddressablesAsset LoadAssetAsync<T>(string name, Action<T> callBack)
        {
            AsyncOperationHandle<T> handle;
            // already loaded before
            if (AssetPool.Instance.Get(name) is AddressablesAsset asset)
            {
                handle = asset.Handle.Convert<T>();
                if (handle.IsDone)
                {
                    callBack(handle.Result);
                }
                else
                {
                    handle.Completed += (obj) =>
                    {
                        if (obj.Status == AsyncOperationStatus.Succeeded)
                            callBack(obj.Result);
                    };
                }

                return asset;
            }

            // has not loaded before
            handle = Addressables.LoadAssetAsync<T>(name);
            handle.Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                    callBack(obj.Result);
                else
                {
                    JLog.Error($"load asset failed: {name}");
                    AssetPool.Instance.Remove(name);
                }
            };

            AddressablesAsset newAsset = new AddressablesAsset(name, handle, typeof(T));
            AssetPool.Instance.Add(newAsset);
            return newAsset;
        }

        #endregion Callback async

        public void Unload<T>(string name)
        {
            AssetPool.Instance.Release(name);
        }

        public void ForceUnloadAsset<T>(string name)
        {
            AssetPool.Instance.UnloadAssetImmediately(name);
        }
    }
}