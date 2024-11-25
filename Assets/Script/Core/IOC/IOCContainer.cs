using System.Collections.Generic;

namespace Jam.Core
{

    public class IOCContainer
    {
        private Dictionary<string, object> _objects;

        public IOCContainer()
        {
            _objects = new Dictionary<string, object>(32);
        }

        public void Register<T>(T obj)
        {
            Register(obj.GetType().Name, obj);
        }

        public void Register<T>(string key, T obj)
        {
            _objects[key] = obj;
        }

        public void Unregister(string key)
        {
            _objects.Remove(key);
        }

        public void Unregister<T>()
        {
            Unregister(typeof(T).Name);
        }

        public bool TryResolve<T>(out T outObj)
        {
            outObj = default;
            if (_objects.TryGetValue(typeof(T).Name, out var obj))
            {
                if (obj is T)
                {
                    outObj = (T)obj;
                    return true;
                }
            }
            return false;
        }

        public T Resolve<T>()
        {
            if (TryResolve(out T obj))
            {
                return obj;
            }
            return default;
        }

        public bool TryResolve<T>(string key, out T outObj)
        {
            outObj = default;
            if (_objects.TryGetValue(key, out var obj))
            {
                if (obj is T)
                {
                    outObj = (T)obj;
                    return true;
                }
            }
            return false;
        }

        public T Resolve<T>(string key)
        {
            if (TryResolve(key, out T obj))
            {
                return obj;
            }
            return default;
        }

        public bool Contains(string key)
        {
            return _objects.ContainsKey(key);
        }

        public bool Contains<T>()
        {
            return Contains(typeof(T).Name);
        }

        public void Clear()
        {
            _objects.Clear();
        }
    }

}