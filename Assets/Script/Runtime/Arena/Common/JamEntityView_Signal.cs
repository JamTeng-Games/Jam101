using System;

namespace Jam.Arena
{

    public partial class JamEntityView
    {
        public void Listen(ViewSignal name, Action action)
        {
            _signal.Listen(name, action);
        }

        public void Listen<T>(ViewSignal name, Action<T> action)
        {
            _signal.Listen(name, action);
        }

        public void Listen<T1, T2>(ViewSignal name, Action<T1, T2> action)
        {
            _signal.Listen(name, action);
        }

        public void Listen<T1, T2, T3>(ViewSignal name, Action<T1, T2, T3> action)
        {
            _signal.Listen(name, action);
        }

        public void ListenObj(ViewSignal name, Action<object> action)
        {
            _signal.ListenObj(name, action);
        }

        public void UnListen(ViewSignal name, Action action)
        {
            _signal.UnListen(name, action);
        }

        public void UnListen<T>(ViewSignal name, Action<T> action)
        {
            _signal.UnListen(name, action);
        }

        public void UnListen<T1, T2>(ViewSignal name, Action<T1, T2> action)
        {
            _signal.UnListen(name, action);
        }

        public void UnListen<T1, T2, T3>(ViewSignal name, Action<T1, T2, T3> action)
        {
            _signal.UnListen(name, action);
        }

        public void UnListenObj(ViewSignal name, Action<object> action)
        {
            _signal.UnListenObj(name, action);
        }

        public void Notify(ViewSignal name)
        {
            _signal.Notify(name);
        }

        public void Notify<T>(ViewSignal name, T arg)
        {
            _signal.Notify(name, arg);
        }

        public void Notify<T1, T2>(ViewSignal name, T1 arg1, T2 arg2)
        {
            _signal.Notify(name, arg1, arg2);
        }

        public void Notify<T1, T2, T3>(ViewSignal name, T1 arg1, T2 arg2, T3 arg3)
        {
            _signal.Notify(name, arg1, arg2, arg3);
        }

        public void NotifyObj(ViewSignal name, object arg)
        {
            _signal.NotifyObj(name, arg);
        }
    }

}