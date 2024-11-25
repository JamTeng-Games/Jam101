namespace Jam.Core
{

    public abstract class Mgr<T> where T : class, new()
    {
        public void Init()
        {
            OnInit();
        }

        public void Shutdown(bool isAppQuit)
        {
            OnShutdown(isAppQuit);
        }

        public abstract void OnInit();
        public abstract void OnShutdown(bool isAppQuit);
    }

}