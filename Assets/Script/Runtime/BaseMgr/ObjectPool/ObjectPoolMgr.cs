using System;
using System.Collections.Generic;
using Jam.Core;

namespace Jam.Runtime.ObjectPool
{

    /// <summary>
    /// 对象池管理器。
    /// </summary>
    public sealed partial class ObjectPoolMgr : IObjectPoolMgr
    {
        private const int DefaultCapacity = int.MaxValue;
        private const float DefaultExpireTime = float.MaxValue;
        private const int DefaultPriority = 0;

        private readonly Dictionary<TypeNamePair, ObjectPoolBase> _objectPools;
        private readonly List<ObjectPoolBase> _cachedAllObjectPools;
        private readonly Comparison<ObjectPoolBase> _objectPoolComparer;

        public int Count => _objectPools.Count;

        /// <summary>
        /// 初始化对象池管理器的新实例。
        /// </summary>
        public ObjectPoolMgr()
        {
            _objectPools = new Dictionary<TypeNamePair, ObjectPoolBase>();
            _cachedAllObjectPools = new List<ObjectPoolBase>();
            _objectPoolComparer = ObjectPoolComparer;
        }

        public void Shutdown(bool isAppQuit)
        {
            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in _objectPools)
            {
                objectPool.Value.Shutdown();
            }

            _objectPools.Clear();
            _cachedAllObjectPools.Clear();
        }

        public void Tick(float deltaTime)
        {
            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in _objectPools)
            {
                objectPool.Value.Tick(deltaTime);
            }
        }

        public bool HasObjectPool<T>() where T : ObjectBase
        {
            return InternalHasObjectPool(new TypeNamePair(typeof(T)));
        }

        public bool HasObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new Exception("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new Exception(
                    Util.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return InternalHasObjectPool(new TypeNamePair(objectType));
        }

        public bool HasObjectPool<T>(string name) where T : ObjectBase
        {
            return InternalHasObjectPool(new TypeNamePair(typeof(T), name));
        }

        public bool HasObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new Exception("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new Exception(
                    Util.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return InternalHasObjectPool(new TypeNamePair(objectType, name));
        }

        public bool HasObjectPool(Predicate<ObjectPoolBase> condition)
        {
            if (condition == null)
            {
                throw new Exception("Condition is invalid.");
            }

            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in _objectPools)
            {
                if (condition(objectPool.Value))
                {
                    return true;
                }
            }

            return false;
        }

        public IObjectPool<T> GetObjectPool<T>() where T : ObjectBase
        {
            return (IObjectPool<T>)InternalGetObjectPool(new TypeNamePair(typeof(T)));
        }

        public ObjectPoolBase GetObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new Exception("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new Exception(
                    Util.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return InternalGetObjectPool(new TypeNamePair(objectType));
        }

        public IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase
        {
            return (IObjectPool<T>)InternalGetObjectPool(new TypeNamePair(typeof(T), name));
        }

        public ObjectPoolBase GetObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new Exception("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new Exception(
                    Util.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return InternalGetObjectPool(new TypeNamePair(objectType, name));
        }

        public ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase> condition)
        {
            if (condition == null)
            {
                throw new Exception("Condition is invalid.");
            }

            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in _objectPools)
            {
                if (condition(objectPool.Value))
                {
                    return objectPool.Value;
                }
            }

            return null;
        }

        public ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase> condition)
        {
            if (condition == null)
            {
                throw new Exception("Condition is invalid.");
            }

            List<ObjectPoolBase> results = new List<ObjectPoolBase>();
            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in _objectPools)
            {
                if (condition(objectPool.Value))
                {
                    results.Add(objectPool.Value);
                }
            }

            return results.ToArray();
        }

        public void GetObjectPools(Predicate<ObjectPoolBase> condition, List<ObjectPoolBase> results)
        {
            if (condition == null)
            {
                throw new Exception("Condition is invalid.");
            }

            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in _objectPools)
            {
                if (condition(objectPool.Value))
                {
                    results.Add(objectPool.Value);
                }
            }
        }

        public ObjectPoolBase[] GetAllObjectPools()
        {
            return GetAllObjectPools(false);
        }

        /// <summary>
        /// 获取所有对象池。
        /// </summary>
        /// <param name="results">所有对象池。</param>
        public void GetAllObjectPools(List<ObjectPoolBase> results)
        {
            GetAllObjectPools(false, results);
        }

        public ObjectPoolBase[] GetAllObjectPools(bool sort)
        {
            if (sort)
            {
                List<ObjectPoolBase> results = new List<ObjectPoolBase>();
                foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in _objectPools)
                {
                    results.Add(objectPool.Value);
                }

                results.Sort(_objectPoolComparer);
                return results.ToArray();
            }
            else
            {
                int index = 0;
                ObjectPoolBase[] results = new ObjectPoolBase[_objectPools.Count];
                foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in _objectPools)
                {
                    results[index++] = objectPool.Value;
                }

                return results;
            }
        }

        public void GetAllObjectPools(bool sort, List<ObjectPoolBase> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in _objectPools)
            {
                results.Add(objectPool.Value);
            }

            if (sort)
            {
                results.Sort(_objectPoolComparer);
            }
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>() where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, DefaultExpireTime, DefaultCapacity,
                                               DefaultExpireTime, DefaultPriority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, DefaultExpireTime, DefaultCapacity,
                                            DefaultExpireTime, DefaultPriority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, DefaultExpireTime, DefaultCapacity, DefaultExpireTime,
                                               DefaultPriority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name)
        {
            return InternalCreateObjectPool(objectType, name, false, DefaultExpireTime, DefaultCapacity,
                                            DefaultExpireTime, DefaultPriority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, DefaultExpireTime, capacity, DefaultExpireTime,
                                               DefaultPriority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, DefaultExpireTime, capacity,
                                            DefaultExpireTime, DefaultPriority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, expireTime, DefaultCapacity, expireTime,
                                               DefaultPriority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, float expireTime)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, expireTime, DefaultCapacity, expireTime,
                                            DefaultPriority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, DefaultExpireTime, capacity, DefaultExpireTime,
                                               DefaultPriority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity)
        {
            return InternalCreateObjectPool(objectType, name, false, DefaultExpireTime, capacity, DefaultExpireTime,
                                            DefaultPriority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, expireTime, DefaultCapacity, expireTime, DefaultPriority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float expireTime)
        {
            return InternalCreateObjectPool(objectType, name, false, expireTime, DefaultCapacity, expireTime,
                                            DefaultPriority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, expireTime, capacity, expireTime, DefaultPriority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, float expireTime)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, expireTime, capacity, expireTime,
                                            DefaultPriority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, DefaultExpireTime, capacity, DefaultExpireTime,
                                               priority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, int priority)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, DefaultExpireTime, capacity,
                                            DefaultExpireTime, priority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(float expireTime, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, expireTime, DefaultCapacity, expireTime, priority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, expireTime, DefaultCapacity, expireTime,
                                            priority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, float expireTime)
            where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, expireTime, capacity, expireTime, DefaultPriority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, float expireTime)
        {
            return InternalCreateObjectPool(objectType, name, false, expireTime, capacity, expireTime, DefaultPriority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, int priority)
            where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, DefaultExpireTime, capacity, DefaultExpireTime, priority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, int priority)
        {
            return InternalCreateObjectPool(objectType, name, false, DefaultExpireTime, capacity, DefaultExpireTime,
                                            priority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float expireTime, int priority)
            where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, expireTime, DefaultCapacity, expireTime, priority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, name, false, expireTime, DefaultCapacity, expireTime, priority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, float expireTime, int priority)
            where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, expireTime, capacity, expireTime, priority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, expireTime, capacity, expireTime,
                                            priority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, float expireTime, int priority)
            where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, expireTime, capacity, expireTime, priority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType,
                                                          string name,
                                                          int capacity,
                                                          float expireTime,
                                                          int priority)
        {
            return InternalCreateObjectPool(objectType, name, false, expireTime, capacity, expireTime, priority);
        }

        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name,
                                                             float autoReleaseInterval,
                                                             int capacity,
                                                             float expireTime,
                                                             int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, autoReleaseInterval, capacity, expireTime, priority);
        }

        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType,
                                                          string name,
                                                          float autoReleaseInterval,
                                                          int capacity,
                                                          float expireTime,
                                                          int priority)
        {
            return InternalCreateObjectPool(objectType, name, false, autoReleaseInterval, capacity, expireTime,
                                            priority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>() where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, DefaultExpireTime, DefaultCapacity,
                                               DefaultExpireTime, DefaultPriority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, DefaultExpireTime, DefaultCapacity,
                                            DefaultExpireTime, DefaultPriority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, DefaultExpireTime, DefaultCapacity, DefaultExpireTime,
                                               DefaultPriority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name)
        {
            return InternalCreateObjectPool(objectType, name, true, DefaultExpireTime, DefaultCapacity,
                                            DefaultExpireTime, DefaultPriority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, DefaultExpireTime, capacity, DefaultExpireTime,
                                               DefaultPriority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, DefaultExpireTime, capacity,
                                            DefaultExpireTime, DefaultPriority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, expireTime, DefaultCapacity, expireTime,
                                               DefaultPriority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, float expireTime)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, expireTime, DefaultCapacity, expireTime,
                                            DefaultPriority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, DefaultExpireTime, capacity, DefaultExpireTime,
                                               DefaultPriority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity)
        {
            return InternalCreateObjectPool(objectType, name, true, DefaultExpireTime, capacity, DefaultExpireTime,
                                            DefaultPriority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, expireTime, DefaultCapacity, expireTime, DefaultPriority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float expireTime)
        {
            return InternalCreateObjectPool(objectType, name, true, expireTime, DefaultCapacity, expireTime,
                                            DefaultPriority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, expireTime, capacity, expireTime, DefaultPriority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, float expireTime)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, expireTime, capacity, expireTime,
                                            DefaultPriority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, DefaultExpireTime, capacity, DefaultExpireTime,
                                               priority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, int priority)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, DefaultExpireTime, capacity,
                                            DefaultExpireTime, priority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(float expireTime, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, expireTime, DefaultCapacity, expireTime, priority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, expireTime, DefaultCapacity, expireTime,
                                            priority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, float expireTime)
            where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, expireTime, capacity, expireTime, DefaultPriority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, float expireTime)
        {
            return InternalCreateObjectPool(objectType, name, true, expireTime, capacity, expireTime, DefaultPriority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, int priority)
            where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, DefaultExpireTime, capacity, DefaultExpireTime, priority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, int priority)
        {
            return InternalCreateObjectPool(objectType, name, true, DefaultExpireTime, capacity, DefaultExpireTime,
                                            priority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float expireTime, int priority)
            where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, expireTime, DefaultCapacity, expireTime, priority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, name, true, expireTime, DefaultCapacity, expireTime, priority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, float expireTime, int priority)
            where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, expireTime, capacity, expireTime, priority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, expireTime, capacity, expireTime, priority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, float expireTime, int priority)
            where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, expireTime, capacity, expireTime, priority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType,
                                                         string name,
                                                         int capacity,
                                                         float expireTime,
                                                         int priority)
        {
            return InternalCreateObjectPool(objectType, name, true, expireTime, capacity, expireTime, priority);
        }

        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name,
                                                            float autoReleaseInterval,
                                                            int capacity,
                                                            float expireTime,
                                                            int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, autoReleaseInterval, capacity, expireTime, priority);
        }

        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType,
                                                         string name,
                                                         float autoReleaseInterval,
                                                         int capacity,
                                                         float expireTime,
                                                         int priority)
        {
            return InternalCreateObjectPool(objectType, name, true, autoReleaseInterval, capacity, expireTime,
                                            priority);
        }

        public bool DestroyObjectPool<T>() where T : ObjectBase
        {
            return InternalDestroyObjectPool(new TypeNamePair(typeof(T)));
        }

        public bool DestroyObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new Exception("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new Exception(
                    Util.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return InternalDestroyObjectPool(new TypeNamePair(objectType));
        }

        public bool DestroyObjectPool<T>(string name) where T : ObjectBase
        {
            return InternalDestroyObjectPool(new TypeNamePair(typeof(T), name));
        }

        public bool DestroyObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new Exception("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new Exception(
                    Util.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return InternalDestroyObjectPool(new TypeNamePair(objectType, name));
        }

        public bool DestroyObjectPool<T>(IObjectPool<T> objectPool) where T : ObjectBase
        {
            if (objectPool == null)
            {
                throw new Exception("Object pool is invalid.");
            }

            return InternalDestroyObjectPool(new TypeNamePair(typeof(T), objectPool.Name));
        }

        public bool DestroyObjectPool(ObjectPoolBase objectPool)
        {
            if (objectPool == null)
            {
                throw new Exception("Object pool is invalid.");
            }

            return InternalDestroyObjectPool(new TypeNamePair(objectPool.ObjectType, objectPool.Name));
        }

        public void Release()
        {
            GetAllObjectPools(true, _cachedAllObjectPools);
            foreach (ObjectPoolBase objectPool in _cachedAllObjectPools)
            {
                objectPool.Release();
            }
        }

        public void ReleaseAllUnused()
        {
            GetAllObjectPools(true, _cachedAllObjectPools);
            foreach (ObjectPoolBase objectPool in _cachedAllObjectPools)
            {
                objectPool.ReleaseAllUnused();
            }
        }

        private bool InternalHasObjectPool(TypeNamePair typeNamePair)
        {
            return _objectPools.ContainsKey(typeNamePair);
        }

        private ObjectPoolBase InternalGetObjectPool(TypeNamePair typeNamePair)
        {
            ObjectPoolBase objectPool = null;
            if (_objectPools.TryGetValue(typeNamePair, out objectPool))
            {
                return objectPool;
            }

            return null;
        }

        private IObjectPool<T> InternalCreateObjectPool<T>(string name,
                                                           bool allowMultiSpawn,
                                                           float autoReleaseInterval,
                                                           int capacity,
                                                           float expireTime,
                                                           int priority) where T : ObjectBase
        {
            TypeNamePair typeNamePair = new TypeNamePair(typeof(T), name);
            if (HasObjectPool<T>(name))
            {
                throw new Exception(Util.Text.Format("Already exist object pool '{0}'.", typeNamePair));
            }

            ObjectPool<T> objectPool =
                new ObjectPool<T>(name, allowMultiSpawn, autoReleaseInterval, capacity, expireTime, priority);
            _objectPools.Add(typeNamePair, objectPool);
            return objectPool;
        }

        private ObjectPoolBase InternalCreateObjectPool(Type objectType,
                                                        string name,
                                                        bool allowMultiSpawn,
                                                        float autoReleaseInterval,
                                                        int capacity,
                                                        float expireTime,
                                                        int priority)
        {
            if (objectType == null)
            {
                throw new Exception("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new Exception(
                    Util.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            TypeNamePair typeNamePair = new TypeNamePair(objectType, name);
            if (HasObjectPool(objectType, name))
            {
                throw new Exception(Util.Text.Format("Already exist object pool '{0}'.", typeNamePair));
            }

            Type objectPoolType = typeof(ObjectPool<>).MakeGenericType(objectType);
            ObjectPoolBase objectPool = (ObjectPoolBase)Activator.CreateInstance(
                objectPoolType, name, allowMultiSpawn, autoReleaseInterval, capacity, expireTime, priority);
            _objectPools.Add(typeNamePair, objectPool);
            return objectPool;
        }

        private bool InternalDestroyObjectPool(TypeNamePair typeNamePair)
        {
            if (_objectPools.TryGetValue(typeNamePair, out ObjectPoolBase objectPool))
            {
                objectPool.Shutdown();
                return _objectPools.Remove(typeNamePair);
            }

            return false;
        }

        private static int ObjectPoolComparer(ObjectPoolBase a, ObjectPoolBase b)
        {
            return a.Priority.CompareTo(b.Priority);
        }
    }

}