using Jam.Core;

namespace Jam.Runtime.IOC
{

    public class IOCMgr : IMgr
    {
        private IOCContainer _container;

        public IOCMgr()
        {
            _container = new IOCContainer();
        }

        public void Shutdown(bool isAppQuit)
        {
            _container.Clear();
            _container = null;
        }

        public void Register<T>(T obj)
        {
            _container.Register(obj);
        }

        public void Register<T>(string key, T obj)
        {
            _container.Register(key, obj);
        }

        public void Unregister(string key)
        {
            _container.Unregister(key);
        }

        public void Unregister<T>()
        {
            _container.Unregister<T>();
        }

        public bool TryResolve<T>(out T outObj)
        {
            return _container.TryResolve(out outObj);
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public bool TryResolve<T>(string key, out T outObj)
        {
            return _container.TryResolve(key, out outObj);
        }

        public T Resolve<T>(string key)
        {
            return _container.Resolve<T>(key);
        }

        public bool Contains(string key)
        {
            return _container.Contains(key);
        }

        public bool Contains<T>()
        {
            return _container.Contains<T>();
        }

        public void Clear()
        {
            _container.Clear();
        }
    }

}