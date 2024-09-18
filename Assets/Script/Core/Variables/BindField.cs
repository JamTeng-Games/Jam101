using System;

namespace J.Core
{

    /// NOTE: Use BindFieldHelper class to bind field
    public class BindField<T>
    {
        private event Action<T> OnValueChanged;
        private T _value;
        private bool _isNull = true;

        public static BindField<T> Create()
        {
            // Log.Error($"Create BindField<{typeof(T).Name}> from pool, you have better manually dispose it");
            BindField<T> field = new BindField<T>();
            return field;
        }

        public void BindListener(Action<T> listener)
        {
            OnValueChanged += listener;
            if (!_isNull)
                listener(_value);
        }

        public void UnbindListener(Action<T> listener)
        {
            OnValueChanged -= listener;
        }

        public void ClearBind()
        {
            OnValueChanged = null;
        }

        public T Value
        {
            get => _value;
            set => OnSet(value, _value);
        }

        protected virtual void OnSet(T newValue, T oldValue)
        {
            if (_value == null)
            {
                _value = newValue;
            }
            else
            {
                if (_value.Equals(newValue))
                    return;
                _value = newValue;
            }
            _isNull = false;
            OnValueChanged?.Invoke(newValue);
        }

        public override string ToString()
        {
            return (_value != null) ? _value.ToString() : "<Null>";
        }

        // void IReference.Clear()
        // {
        //     ClearBind();
        //     _value = default;
        //     _isNull = true;
        // }

        public void Dispose()
        {
            // ReferencePool.Release(this);
        }

        public static explicit operator BindField<T>(T value)
        {
            JLog.Warning($"Explicit cast type <{typeof(T)}> to BindField<>");
            BindField<T> field = new BindField<T>(); //ReferencePool.Get<BindField<T>>();
            field.Value = value;
            return field;
        }

        public static implicit operator T(BindField<T> field)
        {
            return field.Value;
        }
    }

}