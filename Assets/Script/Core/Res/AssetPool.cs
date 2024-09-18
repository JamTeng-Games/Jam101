using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace J.Core
{

    public class AssetPool : LazySingleton<AssetPool>
    {
        private Dictionary<string, IAsset> _assets;
        private Dictionary<string, IAsset> _unusedAssets;

        public int UsingCount => _assets.Count;
        public int UnusedCount => _unusedAssets.Count;

        public AssetPool()
        {
            _assets = new Dictionary<string, IAsset>();
            _unusedAssets = new Dictionary<string, IAsset>();
        }

        public void Add(IAsset asset)
        {
            _assets.TryAdd(asset.Name, asset);
        }

        public void Remove(string keyName)
        {
            _assets.Remove(keyName);
            _unusedAssets.Remove(keyName);
        }

        public bool TryGet(string keyName, out IAsset asset)
        {
            if (_assets.TryGetValue(keyName, out asset))
            {
                asset.AddRef();
                return true;
            }

            if (_unusedAssets.TryGetValue(keyName, out asset))
            {
                asset.ResetRef();
                asset.AddRef();
                _unusedAssets.Remove(keyName);
                _assets.Add(keyName, asset);
                return true;
            }

            return false;
        }

        public IAsset Get(string keyName)
        {
            if (TryGet(keyName, out IAsset asset))
                return asset;

            return null;
        }

        public void Release(string keyName)
        {
            if (_assets.TryGetValue(keyName, out IAsset asset))
            {
                asset.ReduceRef();
                if (asset.RefCount == 0)
                {
                    _assets.Remove(keyName);
                    // _unusedAssets.Add(keyName, asset);
                    UnloadAssetImpl(asset);
                }
            }
            else
            {
                JLog.Warning($"AssetPool doesn't contains asset \"{AddressablesTool.ToRawName(keyName)}\"");
            }
        }

        public bool Contains(string name)
        {
            return _assets.ContainsKey(name) || _unusedAssets.ContainsKey(name);
        }

        public void UnloadAll()
        {
            // Unload unusedAssets
            foreach ((string name, IAsset asset) in _unusedAssets)
            {
                UnloadAssetImpl(asset);
            }
            _unusedAssets.Clear();

            // Unload usingAssets
            foreach ((string name, IAsset asset) in _assets)
            {
                UnloadAssetImpl(asset);
            }
            _assets.Clear();

            AssetBundle.UnloadAllAssetBundles(true);
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        public void UnloadUnusedAssets()
        {
            foreach ((string name, IAsset asset) in _unusedAssets)
            {
                UnloadAssetImpl(asset);
            }
            _unusedAssets.Clear();
            Resources.UnloadUnusedAssets();
        }

        public void UnloadAssetImmediately(string keyName)
        {
            if (_assets.TryGetValue(keyName, out IAsset asset))
            {
                _assets.Remove(keyName);
                UnloadAssetImpl(asset);
                return;
            }

            if (_unusedAssets.TryGetValue(keyName, out asset))
            {
                _unusedAssets.Remove(keyName);
                UnloadAssetImpl(asset);
                return;
            }

            JLog.Warning($"AssetPool doesn't contains asset \"{AddressablesTool.ToRawName(keyName)}\"");
        }

        private void UnloadAssetImpl(IAsset asset)
        {
            asset.ResetRef();
            if (asset.StoreType == StoreType.Resources)
            {
                Resources.UnloadAsset(asset.Res as Object);
            }
            else if (asset.StoreType == StoreType.Addressables)
            {
                Addressables.Release((asset as AddressablesAsset).Handle);
            }
        }
    }

}