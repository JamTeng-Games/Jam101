using System;
using System.Collections.Generic;
using Jam.Core;
using UnityEngine;

namespace Jam.Runtime.ObjectPool
{

    public sealed partial class ObjectPoolMgr : IObjectPoolMgr
    {
        private sealed class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : ObjectBase
        {
            private readonly GFMultiDictionary<string, Object<T>> _objects;
            private readonly Dictionary<object, Object<T>> _objectMap;
            private readonly ReleaseObjectFilterCallback<T> _defaultReleaseObjectFilterCallback;
            private readonly List<T> _cachedCanReleaseObjects;
            private readonly List<T> _cachedToReleaseObjects;
            private readonly bool _allowMultiSpawn;
            private float _autoReleaseInterval;
            private int _capacity;
            private float _expireTime;
            private int _priority;
            private float _autoReleaseTime;

            public ObjectPool(string name,
                              bool allowMultiSpawn,
                              float autoReleaseInterval,
                              int capacity,
                              float expireTime,
                              int priority)
                : base(name)
            {
                _objects = new GFMultiDictionary<string, Object<T>>();
                _objectMap = new Dictionary<object, Object<T>>();
                _defaultReleaseObjectFilterCallback = DefaultReleaseObjectFilterCallback;
                _cachedCanReleaseObjects = new List<T>();
                _cachedToReleaseObjects = new List<T>();
                _allowMultiSpawn = allowMultiSpawn;
                _autoReleaseInterval = autoReleaseInterval;
                Capacity = capacity;
                ExpireTime = expireTime;
                _priority = priority;
                _autoReleaseTime = 0f;
            }

            public override Type ObjectType => typeof(T);

            public override int Count => _objectMap.Count;

            public override int CanReleaseCount
            {
                get
                {
                    GetCanReleaseObjects(_cachedCanReleaseObjects);
                    return _cachedCanReleaseObjects.Count;
                }
            }

            public override bool AllowMultiSpawn => _allowMultiSpawn;

            public override float AutoReleaseInterval
            {
                get => _autoReleaseInterval;
                set => _autoReleaseInterval = value;
            }

            public override int Capacity
            {
                get => _capacity;
                set
                {
                    if (value < 0)
                    {
                        throw new Exception("Capacity is invalid.");
                    }

                    if (_capacity == value)
                    {
                        return;
                    }

                    _capacity = value;
                    Release();
                }
            }

            public override float ExpireTime
            {
                get => _expireTime;

                set
                {
                    if (value < 0f)
                    {
                        throw new Exception("ExpireTime is invalid.");
                    }

                    if (Mathf.Approximately(ExpireTime, value))
                    {
                        return;
                    }

                    _expireTime = value;
                    Release();
                }
            }

            public override int Priority
            {
                get => _priority;
                set => _priority = value;
            }

            public void Register(T obj, bool spawned)
            {
                if (obj == null)
                {
                    throw new Exception("Object is invalid.");
                }

                Object<T> internalObject = Object<T>.Create(obj, spawned);
                _objects.Add(obj.Name, internalObject);
                _objectMap.Add(obj.Target, internalObject);

                if (Count > _capacity)
                {
                    Release();
                }
            }

            public bool CanSpawn()
            {
                return CanSpawn(string.Empty);
            }

            public bool CanSpawn(string name)
            {
                if (name == null)
                {
                    throw new Exception("Name is invalid.");
                }

                if (_objects.TryGetValue(name, out GFLinkedListRange<Object<T>> objectRange))
                {
                    foreach (Object<T> internalObject in objectRange)
                    {
                        if (_allowMultiSpawn || !internalObject.IsInUse)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            public T Spawn()
            {
                return Spawn(string.Empty);
            }

            public T Spawn(string name)
            {
                if (name == null)
                {
                    throw new Exception("Name is invalid.");
                }

                if (_objects.TryGetValue(name, out GFLinkedListRange<Object<T>> objectRange))
                {
                    foreach (Object<T> internalObject in objectRange)
                    {
                        if (_allowMultiSpawn || !internalObject.IsInUse)
                        {
                            return internalObject.Spawn();
                        }
                    }
                }

                return null;
            }

            public void Unspawn(T obj)
            {
                if (obj == null)
                {
                    throw new Exception("Object is invalid.");
                }

                Unspawn(obj.Target);
            }

            public void Unspawn(object target)
            {
                if (target == null)
                {
                    throw new Exception("Target is invalid.");
                }

                Object<T> internalObject = GetObject(target);
                if (internalObject != null)
                {
                    internalObject.Unspawn();
                    if (Count > _capacity && internalObject.SpawnCount <= 0)
                    {
                        Release();
                    }
                }
                else
                {
                    throw new Exception(Utils.Text.Format(
                                            "Can not find target in object pool '{0}', target type is '{1}', target value is '{2}'.",
                                            new TypeNamePair(typeof(T), Name), target.GetType().FullName, target));
                }
            }

            public void SetLocked(T obj, bool locked)
            {
                if (obj == null)
                {
                    throw new Exception("Object is invalid.");
                }

                SetLocked(obj.Target, locked);
            }

            public void SetLocked(object target, bool locked)
            {
                if (target == null)
                {
                    throw new Exception("Target is invalid.");
                }

                Object<T> internalObject = GetObject(target);
                if (internalObject != null)
                {
                    internalObject.SetLocked(locked);
                }
                else
                {
                    throw new Exception(Utils.Text.Format(
                                            "Can not find target in object pool '{0}', target type is '{1}', target value is '{2}'.",
                                            new TypeNamePair(typeof(T), Name), target.GetType().FullName, target));
                }
            }

            public void SetPriority(T obj, int priority)
            {
                if (obj == null)
                {
                    throw new Exception("Object is invalid.");
                }

                SetPriority(obj.Target, priority);
            }

            public void SetPriority(object target, int priority)
            {
                if (target == null)
                {
                    throw new Exception("Target is invalid.");
                }

                Object<T> internalObject = GetObject(target);
                if (internalObject != null)
                {
                    internalObject.SetPriority(priority);
                }
                else
                {
                    throw new Exception(Utils.Text.Format(
                                            "Can not find target in object pool '{0}', target type is '{1}', target value is '{2}'.",
                                            new TypeNamePair(typeof(T), Name), target.GetType().FullName, target));
                }
            }

            public bool ReleaseObject(T obj)
            {
                if (obj == null)
                {
                    throw new Exception("Object is invalid.");
                }

                return ReleaseObject(obj.Target);
            }

            public bool ReleaseObject(object target)
            {
                if (target == null)
                {
                    throw new Exception("Target is invalid.");
                }

                Object<T> internalObject = GetObject(target);
                if (internalObject == null)
                {
                    return false;
                }

                if (internalObject.IsInUse || internalObject.Locked || !internalObject.CustomCanReleaseFlag)
                {
                    return false;
                }

                _objects.Remove(internalObject.Name, internalObject);
                _objectMap.Remove(internalObject.Peek().Target);

                internalObject.Release(false);
                ReferencePool.Release(internalObject);
                return true;
            }

            public override void Release()
            {
                Release(Count - _capacity, _defaultReleaseObjectFilterCallback);
            }

            public override void Release(int toReleaseCount)
            {
                Release(toReleaseCount, _defaultReleaseObjectFilterCallback);
            }

            public void Release(ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                Release(Count - _capacity, releaseObjectFilterCallback);
            }

            public void Release(int toReleaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                if (releaseObjectFilterCallback == null)
                {
                    throw new Exception("Release object filter callback is invalid.");
                }

                if (toReleaseCount < 0)
                {
                    toReleaseCount = 0;
                }

                DateTime expireTime = DateTime.MinValue;
                if (_expireTime < float.MaxValue)
                {
                    expireTime = DateTime.UtcNow.AddSeconds(-_expireTime);
                }

                _autoReleaseTime = 0f;
                GetCanReleaseObjects(_cachedCanReleaseObjects);
                List<T> toReleaseObjects =
                    releaseObjectFilterCallback(_cachedCanReleaseObjects, toReleaseCount, expireTime);
                if (toReleaseObjects == null || toReleaseObjects.Count <= 0)
                {
                    return;
                }

                foreach (T toReleaseObject in toReleaseObjects)
                {
                    ReleaseObject(toReleaseObject);
                }
            }

            public override void ReleaseAllUnused()
            {
                _autoReleaseTime = 0f;
                GetCanReleaseObjects(_cachedCanReleaseObjects);
                foreach (T toReleaseObject in _cachedCanReleaseObjects)
                {
                    ReleaseObject(toReleaseObject);
                }
            }

            public override ObjectInfo[] GetAllObjectInfos()
            {
                List<ObjectInfo> results = new List<ObjectInfo>();
                foreach (KeyValuePair<string, GFLinkedListRange<Object<T>>> objectRanges in _objects)
                {
                    foreach (Object<T> internalObject in objectRanges.Value)
                    {
                        results.Add(new ObjectInfo(internalObject.Name, internalObject.Locked,
                                                   internalObject.CustomCanReleaseFlag, internalObject.Priority,
                                                   internalObject.LastUseTime, internalObject.SpawnCount));
                    }
                }

                return results.ToArray();
            }

            public override void Tick(float dt)
            {
                _autoReleaseTime += dt;
                if (_autoReleaseTime < _autoReleaseInterval)
                {
                    return;
                }

                Release();
            }

            public override void Shutdown()
            {
                foreach (KeyValuePair<object, Object<T>> objectInMap in _objectMap)
                {
                    objectInMap.Value.Release(true);
                    ReferencePool.Release(objectInMap.Value);
                }

                _objects.Clear();
                _objectMap.Clear();
                _cachedCanReleaseObjects.Clear();
                _cachedToReleaseObjects.Clear();
            }

            private Object<T> GetObject(object target)
            {
                if (target == null)
                {
                    throw new Exception("Target is invalid.");
                }

                if (_objectMap.TryGetValue(target, out Object<T> internalObject))
                {
                    return internalObject;
                }

                return null;
            }

            private void GetCanReleaseObjects(List<T> results)
            {
                if (results == null)
                {
                    throw new Exception("Results is invalid.");
                }

                results.Clear();
                foreach (KeyValuePair<object, Object<T>> objectInMap in _objectMap)
                {
                    Object<T> internalObject = objectInMap.Value;
                    if (internalObject.IsInUse || internalObject.Locked || !internalObject.CustomCanReleaseFlag)
                    {
                        continue;
                    }

                    results.Add(internalObject.Peek());
                }
            }

            private List<T> DefaultReleaseObjectFilterCallback(List<T> candidateObjects,
                                                               int toReleaseCount,
                                                               DateTime expireTime)
            {
                _cachedToReleaseObjects.Clear();

                // 超时的
                if (expireTime > DateTime.MinValue)
                {
                    for (int i = candidateObjects.Count - 1; i >= 0; i--)
                    {
                        if (candidateObjects[i].LastUseTime <= expireTime)
                        {
                            _cachedToReleaseObjects.Add(candidateObjects[i]);
                            candidateObjects.RemoveAt(i);
                            continue;
                        }
                    }

                    toReleaseCount -= _cachedToReleaseObjects.Count;
                }

                // 没超时，按优先级释放
                for (int i = 0; toReleaseCount > 0 && i < candidateObjects.Count; i++)
                {
                    // 冒泡
                    for (int j = i + 1; j < candidateObjects.Count; j++)
                    {
                        if (candidateObjects[i].Priority > candidateObjects[j].Priority ||
                            candidateObjects[i].Priority == candidateObjects[j].Priority &&
                            candidateObjects[i].LastUseTime > candidateObjects[j].LastUseTime)
                        {
                            (candidateObjects[i], candidateObjects[j]) = (candidateObjects[j], candidateObjects[i]);
                        }
                    }

                    _cachedToReleaseObjects.Add(candidateObjects[i]);
                    toReleaseCount--;
                }

                return _cachedToReleaseObjects;
            }
        }
    }

}