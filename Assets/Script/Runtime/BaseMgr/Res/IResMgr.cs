using Cysharp.Threading.Tasks;
using UnityEngine;

namespace J.Runtime.Res
{
    public interface IResMgr
    {
        public UniTask WaitForAssetInit();
        public UniTask<T> Load<T>(string path) where T : Object;
        public T SyncLoad<T>(string path) where T : Object;
        public void Unload(string path);
        public void Init();
        public void Shutdown();
    }
}