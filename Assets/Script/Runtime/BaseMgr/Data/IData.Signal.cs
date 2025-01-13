using System;

namespace Jam.Runtime.Data_
{

    public abstract partial class IData
    {
        public void Listen(DataSignalType name, Action action)
        {
            _signal.Listen(name, action);
        }

        public void Listen<T>(DataSignalType name, Action<T> action)
        {
            _signal.Listen(name, action);
        }

        public void Listen<T1, T2>(DataSignalType name, Action<T1, T2> action)
        {
            _signal.Listen(name, action);
        }

        public void Listen<T1, T2, T3>(DataSignalType name, Action<T1, T2, T3> action)
        {
            _signal.Listen(name, action);
        }

        public void UnListen(DataSignalType name, Action action)
        {
            _signal.UnListen(name, action);
        }

        public void UnListen<T>(DataSignalType name, Action<T> action)
        {
            _signal.UnListen(name, action);
        }

        public void UnListen<T1, T2>(DataSignalType name, Action<T1, T2> action)
        {
            _signal.UnListen(name, action);
        }

        public void UnListen<T1, T2, T3>(DataSignalType name, Action<T1, T2, T3> action)
        {
            _signal.UnListen(name, action);
        }

        public void Notify(DataSignalType name)
        {
            _signal.Notify(name);
        }

        public void Notify<T>(DataSignalType name, T arg)
        {
            _signal.Notify(name, arg);
        }

        public void Notify<T1, T2>(DataSignalType name, T1 arg1, T2 arg2)
        {
            _signal.Notify(name, arg1, arg2);
        }

        public void Notify<T1, T2, T3>(DataSignalType name, T1 arg1, T2 arg2, T3 arg3)
        {
            _signal.Notify(name, arg1, arg2, arg3);
        }
    }

}