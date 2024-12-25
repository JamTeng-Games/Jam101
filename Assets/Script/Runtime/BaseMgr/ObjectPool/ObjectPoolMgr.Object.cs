using System;
using Jam.Core;

namespace Jam.Runtime.ObjectPool
{

    public sealed partial class ObjectPoolMgr : IObjectPoolMgr
    {
        private sealed class Object<T> : IReference where T : ObjectBase
        {
            private T _object;
            private int _spawnCount;

            public string Name => _object.Name;
            public bool Locked => _object.Locked;
            public int Priority => _object.Priority;
            public bool CustomCanReleaseFlag => _object.CustomCanReleaseFlag;
            public DateTime LastUseTime => _object.LastUseTime;
            public bool IsInUse => _spawnCount > 0;
            public int SpawnCount => _spawnCount;

            public Object()
            {
                _object = null;
                _spawnCount = 0;
            }

            public static Object<T> Create(T obj, bool spawned)
            {
                if (obj == null)
                {
                    throw new Exception("Object is invalid.");
                }

                Object<T> internalObject = ReferencePool.Get<Object<T>>();
                internalObject._object = obj;
                internalObject._spawnCount = spawned ? 1 : 0;
                if (spawned)
                {
                    obj.OnSpawn();
                }

                return internalObject;
            }

            public void Clean()
            {
                _object = null;
                _spawnCount = 0;
            }

            public T Peek()
            {
                return _object;
            }

            public T Spawn()
            {
                _spawnCount++;
                _object.SetLastUseTime(DateTime.UtcNow);
                _object.OnSpawn();
                return _object;
            }

            public void Unspawn()
            {
                _object.OnUnspawn();
                _object.SetLastUseTime(DateTime.UtcNow);
                _spawnCount--;
                if (_spawnCount < 0)
                {
                    throw new Exception(Utils.Text.Format("Object '{0}' spawn count is less than 0.", Name));
                }
            }

            public void Release(bool isShutdown)
            {
                _object.Release(isShutdown);
                ReferencePool.Release(_object);
            }

            public void SetLocked(bool locked)
            {
                _object.SetLocked(locked);
            }

            public void SetPriority(int priority)
            {
                _object.SetPriority(priority);
            }
        }
    }

}