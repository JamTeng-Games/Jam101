using Cysharp.Threading.Tasks;
using Jam.Core;
using UnityEngine;

namespace Jam.Runtime.Asset
{

    public interface IAssetMgr : IMgr
    {
        public void Init();

        public UniTask<AssetHandleWrap> Load(string assetPath, System.Type objType);

        public void Load(string assetPath,
                         System.Type objType,
                         System.Action<AssetHandleWrap> callback,
                         object userData);

        public void Unload(int assetHandleId);
        public Object SyncLoad(string assetPath, System.Type objType, out int handleId);
    }

}