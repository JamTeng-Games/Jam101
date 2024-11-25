// using System;
// using Cysharp.Threading.Tasks;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace Jam.Core
// {
//     public class ResourceAsset<T> : Asset
//     {
//         private object _asset;
//
//         public override object Res => _asset;
//         public override StoreType StoreType => StoreType.Resources;
//         public override bool IsDone => true;
//
//         public ResourceAsset(string name, T asset) : base(name, typeof(T))
//         {
//             _asset = asset;
//         }
//     }
//
//     public class ResourcesTool
//     {
//         public T LoadAsset<T>(string path) where T : Object
//         {
//             // AssetPool contains asset
//             if (AssetPool.Instance.TryGet(path, out IAsset asset))
//                 return (T)asset.Res;
//
//             // AssetPool DOES NOT contains the asset
//             T res = Resources.Load<T>(path);
//             if (res == null)
//             {
//                 JLog.Error($"Load failed, Resources folder doesn't contain this asset: {path})");
//                 return null;
//             }
//
//             asset = new ResourceAsset<T>(path, res);
//             AssetPool.Instance.Add(asset);
//             return res;
//         }
//
//         public async UniTask<UniTask<T>> LoadFromAD<T>(string path) where T : Object
//         {
//             var utcs = new UniTaskCompletionSource<T>();
//             // AssetPool contains asset
//             if (AssetPool.Instance.TryGet(path, out IAsset assetInPool))
//             {
//                 utcs.TrySetResult((T)assetInPool.Res);
//                 return utcs.Task;
//             }
//
//             // Load Async
//             var loadedAsset = await Resources.LoadAsync<T>(path);
//             if (loadedAsset == null)
//             {
//                 Exception e = new Exception($"Load failed, Resources folder doesn't contain this asset: {path}");
//                 JLog.Exception(e);
//                 utcs.TrySetException(e);
//                 return utcs.Task;
//             }
//
//             // Load success, add into pool
//             T res = loadedAsset as T;
//             assetInPool = new ResourceAsset<T>(path, res);
//             AssetPool.Instance.Add(assetInPool);
//             utcs.TrySetResult(res);
//             return utcs.Task;
//         }
//
//         public void Unload(string path)
//         {
//             AssetPool.Instance.Release(path);
//         }
//     }
// }