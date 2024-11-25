// using System.Collections.Generic;
// using Cysharp.Threading.Tasks;
// using Jam.Core;
// using UnityEngine;
// using xasset;
// using Asset = xasset.Asset;
//
// namespace Jam.Runtime.Res
// {
//     // TODO: 缓存池优化
//     public class XAssetInfo
//     {
//         private int _refCount;
//         private AssetRequest _req;
//
//         public AssetRequest Req => _req;
//         public bool IsDone => _req.isDone;
//         public int RefCount => _refCount;
//         public Object Asset => _req.asset;
//
//         public XAssetInfo(AssetRequest req)
//         {
//             _refCount = 1;
//             _req = req;
//         }
//
//         public void AddRef()
//         {
//             _refCount++;
//         }
//
//         public void ReduceRef()
//         {
//             _refCount--;
//         }
//
//         public void ResetRef()
//         {
//             _refCount = 0;
//         }
//     }
//
//     // xasset
//     public class AssetMgr : IResMgr
//     {
//         private bool _isInit;
//         private Dictionary<string, XAssetInfo> _loadHandler;
//         private Dictionary<string, List<UniTaskCompletionSource>> _queueTasks;
//
//         public void Init()
//         {
//             _loadHandler = new Dictionary<string, XAssetInfo>(512);
//             _queueTasks = new Dictionary<string, List<UniTaskCompletionSource>>(512);
//         }
//
//         public void Shutdown()
//         {
//         }
//
//         public async UniTask WaitForAssetInit()
//         {
//             await Assets.InitializeAsync().ToUniTask();
//         }
//
//         public async UniTask<T> Load<T>(string path) where T : Object
//         {
//             if (_loadHandler.TryGetValue(path, out XAssetInfo info))
//             {
//                 info.AddRef();
//                 if (info.IsDone)
//                 {
//                     return info.Asset as T;
//                 }
//                 else
//                 {
//                     var utcs = CreateNewTask(path);
//                     await utcs.Task;
//                     return _loadHandler[path].Asset as T;
//                 }
//             }
//
//             AssetRequest req = Asset.LoadAsync(path, typeof(T));
//             if (req == null)
//             {
//                 JLog.Error($"Load asset failed: {path} not found.");
//                 return null;
//             }
//
//             _loadHandler.TryAdd(path, new XAssetInfo(req));
//             await req.ToUniTask();
//             T asset = req?.asset as T;
//             if (asset == null)
//                 _loadHandler.Remove(path);
//
//             var taskQueue = GetTaskQueue(path);
//             foreach (var task in taskQueue)
//             {
//                 task.TrySetResult();
//             }
//             ClearTaskQueue(path);
//             return asset;
//         }
//
//         public T SyncLoad<T>(string path) where T : Object
//         {
//             if (_loadHandler.TryGetValue(path, out XAssetInfo info))
//             {
//                 info.AddRef();
//                 if (info.IsDone)
//                 {
//                     return info.Asset as T;
//                 }
//             }
//
//             AssetRequest req = Asset.Load(path, typeof(T));
//             T asset = req?.asset as T;
//             if (asset != null)
//             {
//                 _loadHandler.TryAdd(path, new XAssetInfo(req));
//             }
//             return asset;
//         }
//
//         public void Unload(string path)
//         {
//             if (_loadHandler.TryGetValue(path, out var assetInfo))
//             {
//                 // TODO: 取消加载，释放资源
//                 if (!assetInfo.IsDone)
//                 {
//                     return;
//                 }
//
//                 assetInfo.ReduceRef();
//                 if (assetInfo.RefCount <= 0)
//                 {
//                     assetInfo.Req.Release();
//                     _loadHandler.Remove(path);
//                 }
//             }
//         }
//
//         // Helpers
//         private List<UniTaskCompletionSource> GetTaskQueue(string path)
//         {
//             if (!_queueTasks.TryGetValue(path, out var queue))
//             {
//                 queue = new List<UniTaskCompletionSource>(4);
//                 _queueTasks.Add(path, queue);
//             }
//             return queue;
//         }
//
//         private UniTaskCompletionSource CreateNewTask(string path)
//         {
//             var queue = GetTaskQueue(path);
//             UniTaskCompletionSource utcs = new UniTaskCompletionSource();
//             queue.Add(utcs);
//             return utcs;
//         }
//
//         private void ClearTaskQueue(string path)
//         {
//             if (_queueTasks.TryGetValue(path, out var queue))
//             {
//                 queue.Clear();
//             }
//         }
//     }
// }