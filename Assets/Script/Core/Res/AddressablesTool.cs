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
            string keyName = GetKeyName<T>(name);
            AsyncOperationHandle<T> handle;
            // already loaded before
            if (AssetPool.Instance.Get(keyName) is AddressablesAsset asset)
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
            AddressablesAsset newAsset = new AddressablesAsset(keyName, handle, typeof(T));
            AssetPool.Instance.Add(newAsset);
            return result;
        }

        #region UniTask async

        public UniTask<T> LoadFromAA<T>(string name)
        {
            string keyName = GetKeyName<T>(name);
            AsyncOperationHandle<T> handle;
            // already loaded before
            if (AssetPool.Instance.Get(keyName) is AddressablesAsset asset)
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
                            utcs.TrySetException(new Exception($"load asset failed: {keyName}"));
                    };
                    return utcs.Task;
                }
            }

            // has not loaded before
            handle = Addressables.LoadAssetAsync<T>(name);
            AddressablesAsset newAsset = new AddressablesAsset(keyName, handle, typeof(T));
            AssetPool.Instance.Add(newAsset);
            return handle.ToUniTask<T>();
        }

        public UniTask<IList<T>> LoadAssetsAsync<T>(Addressables.MergeMode mode,
                                                    params string[] keys)
        {
            string keyName = GetKeyName<T>(keys);
            AsyncOperationHandle<IList<T>> handle;
            // already loaded before
            if (AssetPool.Instance.Get(keyName) is AddressablesAsset asset)
            {
                handle = asset.Handle.Convert<IList<T>>();
                if (handle.IsDone)
                {
                    return UniTask.FromResult(handle.Result);
                }
                else
                {
                    var utcs = new UniTaskCompletionSource<IList<T>>();
                    handle.Completed += (obj) =>
                    {
                        if (obj.Status == AsyncOperationStatus.Succeeded)
                            utcs.TrySetResult(obj.Result);
                        else
                            utcs.TrySetException(new Exception($"load asset failed: {keyName}"));
                    };
                    return utcs.Task;
                }
            }

            // has not loaded before
            List<string> list = new List<string>(keys);
            handle = Addressables.LoadAssetsAsync<T>(list, res =>
            {
            }, mode);
            AddressablesAsset newAsset = new AddressablesAsset(keyName, handle, typeof(IList<T>));
            AssetPool.Instance.Add(newAsset);
            return handle.ToUniTask();
        }

        #endregion UniTask async

        #region Callback async

        public AddressablesAsset LoadAssetAsync<T>(string name, Action<T> callBack)
        {
            string keyName = GetKeyName<T>(name);

            AsyncOperationHandle<T> handle;
            // already loaded before
            if (AssetPool.Instance.Get(keyName) is AddressablesAsset asset)
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
                    AssetPool.Instance.Remove(keyName);
                }
            };

            AddressablesAsset newAsset = new AddressablesAsset(keyName, handle, typeof(T));
            AssetPool.Instance.Add(newAsset);
            return newAsset;
        }

        // load multiple assets or specific a asset
        public AddressablesAsset LoadAssetsAsync<T>(Addressables.MergeMode mode,
                                                    Action<T> callBack,
                                                    Action<bool> completeCallback,
                                                    params string[] keys)
        {
            string keyName = GetKeyName<T>(keys);
            AsyncOperationHandle<IList<T>> handle;
            // already loaded before
            if (AssetPool.Instance.Get(keyName) is AddressablesAsset asset)
            {
                handle = asset.Handle.Convert<IList<T>>();
                if (handle.IsDone)
                {
                    foreach (T item in handle.Result)
                        callBack(item);
                    completeCallback?.Invoke(true);
                }
                else
                {
                    handle.Completed += (obj) =>
                    {
                        if (obj.Status == AsyncOperationStatus.Succeeded)
                        {
                            foreach (T item in handle.Result)
                                callBack(item);
                            completeCallback?.Invoke(true);
                        }
                        else if (obj.Status == AsyncOperationStatus.Failed)
                        {
                            completeCallback?.Invoke(false);
                        }
                    };
                }
                return asset;
            }

            // has not loaded before
            List<string> list = new List<string>(keys);
            handle = Addressables.LoadAssetsAsync<T>(list, callBack, mode);
            handle.Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Failed)
                {
                    JLog.Error($"load asset failed: {keyName}");
                    AssetPool.Instance.Remove(keyName);
                    completeCallback?.Invoke(false);
                }
                else if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    completeCallback?.Invoke(true);
                }
            };
            AddressablesAsset newAsset = new AddressablesAsset(keyName, handle, typeof(IList<T>));
            AssetPool.Instance.Add(newAsset);
            return newAsset;
        }

        #endregion Callback async

        public void Unload<T>(string name)
        {
            string keyName = GetKeyName<T>(name);
            AssetPool.Instance.Release(keyName);
        }

        public void UnloadAssets<T>(params string[] keys)
        {
            string keyName = GetKeyName<T>(keys);
            AssetPool.Instance.Release(keyName);
        }

        public void ForceUnloadAsset<T>(string name)
        {
            string keyName = GetKeyName<T>(name);
            AssetPool.Instance.UnloadAssetImmediately(keyName);
        }

        #region Helpers

        private string GetKeyName<T>(string name)
        {
            return $"Ad_{name}_{typeof(T).Name}";
        }

        private string GetKeyName<T>(string[] names)
        {
            StringBuilder keyName = new StringBuilder("Ad_");
            foreach (string name in names)
                keyName.Append($"{name}_");
            keyName.Append(typeof(T).Name);
            return keyName.ToString();
        }

        public static string ToRawName(string keyName)
        {
            int startIndex = keyName.IndexOf("_", StringComparison.Ordinal);
            int endIndex = keyName.LastIndexOf("_", StringComparison.Ordinal);
            if (startIndex == -1 || endIndex == -1 || endIndex <= startIndex)
                return string.Empty;
            return keyName.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        #endregion Helpers
    }

}