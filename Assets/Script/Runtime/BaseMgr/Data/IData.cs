using Jam.Core;

namespace Jam.Runtime.Data_
{

    /// <summary>
    /// 建议: IData 只继承一层
    /// 基本要提供一个初始化数据方法 InitData(args...),
    /// 以及一个更新数据的方法 UpdateData(args...)
    /// </summary>
    public abstract partial class IData : IReference
    { 
        protected Signal<DataSignalType> _signal;

        public static T Create<T>() where T : IData, new()
        {
            T data = ReferencePool.Get<T>();
            return data;
        }

        protected IData()
        {
            _signal = Signal<DataSignalType>.Create<DataSignalType>();
        }

        public void Dispose()
        {
            Detach();
            OnDispose();
            ReferencePool.Release(this);
        }

        public void Attach(IData parent)
        {
            if (parent == null)
            {
                Detach();
                return;
            }
            _signal.Attach(parent._signal);
        }

        public void Detach()
        {
            _signal.Detach();
        }

        protected abstract void OnDispose();

        void IReference.Clean()
        {
        }
    }

}