namespace J.Core
{
    public abstract class Mgr<T> where T : class, new()
    {
        private static T _instance = new T();
        public static T Instance => _instance;

        static Mgr()
        {
        }

        public abstract void Init();
        public abstract void Shutdown();

        public virtual void Tick(float dt)
        {
        }

        public virtual void LateTick()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void OnApplicationQuit()
        {
        }
    }
}