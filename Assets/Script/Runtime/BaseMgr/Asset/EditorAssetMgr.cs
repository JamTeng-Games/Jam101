using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Jam.Runtime.Asset
{

#if UNITY_EDITOR
    public class EditorAssetMgr : IAssetMgr
    {
        private static int _handleId = 0;
        private Dictionary<int, AssetHandleWrap> _handleWraps;

        public EditorAssetMgr()
        {
            _handleWraps = new Dictionary<int, AssetHandleWrap>();
        }

        public void Init()
        {
        }

        public void Shutdown(bool isAppQuit)
        {
            foreach (var (_, wrap) in _handleWraps)
            {
                wrap.Dispose();
            }
        }

        public async UniTask<AssetHandleWrap> Load(string assetPath, Type objType)
        {
            Object res = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, objType);
            var wrap = CreateHandleWrap(res, assetPath, null);
            return wrap;
        }

        public void Load(string assetPath, Type objType, Action<AssetHandleWrap> callback, object userData)
        {
            Object res = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, objType);
            var wrap = CreateHandleWrap(res, assetPath, userData);
            callback?.Invoke(wrap);
        }

        public void Unload(int assetHandleId)
        {
            RemoveHandleWrap(assetHandleId);
        }

        public Object SyncLoad(string assetPath, Type objType, out int handleId)
        {
            Object res = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, objType);
            var wrap = CreateHandleWrap(res, assetPath, null);
            handleId = wrap.Id;
            return res;
        }

        // Helpers
        private AssetHandleWrap GetHandleWrap(int handleId)
        {
            return _handleWraps.GetValueOrDefault(handleId);
        }

        private AssetHandleWrap CreateHandleWrap(Object asset, string assetPath, object userData)
        {
            int handleId = _handleId++;
            return AddHandleWrap(handleId, asset, assetPath, userData);
        }

        private AssetHandleWrap AddHandleWrap(int handleId, Object asset, string assetPath, object userData)
        {
            AssetHandleWrap wrap = AssetHandleWrap.Create(handleId, asset, assetPath, userData);
            _handleWraps.Add(handleId, wrap);
            return wrap;
        }

        private void RemoveHandleWrap(int handleId)
        {
            if (_handleWraps.Remove(handleId, out var wrap))
            {
                wrap.Dispose();
            }
        }
    }
#endif

}