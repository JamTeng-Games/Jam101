using System;
using Cysharp.Threading.Tasks;
using J.Core;
using Object = UnityEngine.Object;

namespace J.Runtime.Res
{

    public static class ResMgr
    {
        private static AddressablesTool _addressablesTool;
        private static ResourcesTool _resourcesTool;
        private static AssetDatabaseTool _assetDatabaseTool;

        static ResMgr()
        {
            _addressablesTool = new AddressablesTool();
            _resourcesTool = new ResourcesTool();
            _assetDatabaseTool = new AssetDatabaseTool();
        }

        public static void Shutdown()
        {
            _addressablesTool = null;
            _resourcesTool = null;
            _assetDatabaseTool = null;
        }

        public static UniTask<T> LoadFromAA<T>(string path)
        {
            try
            {
                return _addressablesTool.LoadFromAA<T>(path);
            }
            catch (Exception ex)
            {
                JLog.Error($"Failed to load asset: {path}. Error: {ex.Message}");
                return default;
            }
        }

        public static T LoadFromAD<T>(string path) where T : Object
        {
            return _assetDatabaseTool.Load<T>(path);
        }

        public static T LoadFromRes<T>(string path) where T : Object
        {
            return _resourcesTool.LoadAsset<T>(path);
        }

        public static T LoadFromAA_Sync<T>(string path) where T : Object
        {
            return _addressablesTool.Load<T>(path);
        }

        public static void UnloadFromAA<T>(string name)
        {
            _addressablesTool.Unload<T>(name);
        }

        public static void UnloadFromAD<T>(string name)
        {
            _assetDatabaseTool.Unload(name);
        }

        public static void UnloadFromRes(string name)
        {
            _resourcesTool.Unload(name);
        }

        public static void UnloadAll()
        {
            AssetPool.Instance.UnloadAll();
        }

        public static void UnloadUnusedAssets()
        {
            AssetPool.Instance.UnloadUnusedAssets();
        }
    }

}