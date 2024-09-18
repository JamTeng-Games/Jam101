using System;

namespace J.Core
{

    public enum AssetType
    {
    }

    public enum StoreType
    {
        Addressables,
        Resources
    }

    public interface IAsset
    {
        public string Name { get; }
        public StoreType StoreType { get; }
        public Type AssetType { get; }
        public object Res { get; }
        public int RefCount { get; }
        public bool IsDone { get; }

        public void AddRef();
        public void ReduceRef();
        public void ResetRef();
    }

    public abstract class Asset : IAsset
    {
        protected string _name;
        protected int _refCount;
        protected Type _assetType;

        public string Name => _name;
        public Type AssetType => _assetType;
        public int RefCount => _refCount;

        public abstract object Res { get; }
        public abstract StoreType StoreType { get; }
        public abstract bool IsDone { get; }

        protected Asset(string name, Type assetType)
        {
            _name = name;
            _assetType = assetType;
            _refCount = 1;
        }

        public void AddRef()
        {
            _refCount++;
        }

        public void ReduceRef()
        {
            // if (_refCount == 0)
            //     return;
            _refCount--;
        }

        public void ResetRef()
        {
            _refCount = 0;
        }
    }

}