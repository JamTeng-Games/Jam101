//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using Jam.Core;

namespace Jam.Runtime.ObjectPool
{

    /// <summary>
    /// 对象池基类。
    /// </summary>
    public abstract class ObjectPoolBase
    {
        private readonly string _name;

        public ObjectPoolBase()
            : this(null)
        {
        }

        public ObjectPoolBase(string name)
        {
            _name = name ?? string.Empty;
        }

        public string Name => _name;

        public string FullName => new TypeNamePair(ObjectType, _name).ToString();

        public abstract Type ObjectType { get; }

        public abstract int Count { get; }

        public abstract int CanReleaseCount { get; }

        public abstract bool AllowMultiSpawn { get; }

        public abstract float AutoReleaseInterval { get; set; }

        public abstract int Capacity { get; set; }

        public abstract float ExpireTime { get; set; }

        public abstract int Priority { get; set; }

        /// 释放对象池中的可释放对象。
        public abstract void Release();

        /// 释放对象池中的可释放对象。
        public abstract void Release(int toReleaseCount);

        /// 释放对象池中的所有未使用对象。
        public abstract void ReleaseAllUnused();

        /// 获取所有对象信息。
        public abstract ObjectInfo[] GetAllObjectInfos();

        public abstract void Tick(float deltaTime);

        public abstract void Shutdown();
    }

}