using System;
using System.Collections;
using System.Collections.Generic;

namespace J.Core
{

    public interface IHashBind<TKey, TValue>
    {
        public void OnAdd(TKey key, TValue value);
        public void OnRemove(TKey key);
        public void OnChange(Dictionary<TKey, TValue> value);
        public void OnUpdate(TKey key, TValue value);
        public void OnClear();
    }

    /// NOTE: Use BindHashHelper class to bind hash
    public class BindHash<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private Dictionary<TKey, TValue> _values = new Dictionary<TKey, TValue>();

        private event Action<TKey, TValue> OnAdd;
        private event Action<TKey> OnRemove;
        private event Action<TKey, TValue> OnUpdate;
        private event Action OnClear;
        private event Action<Dictionary<TKey, TValue>> OnChange;

        public int Count => _values.Count;

        public static BindHash<TKey, TValue> Create()
        {
            BindHash<TKey, TValue> hashMap = new BindHash<TKey, TValue>(); // ReferencePool.Get<BindHash<TKey, TValue>>();
            return hashMap;
        }

        public void Dispose()
        {
            // ReferencePool.Release(this);
        }

        #region Bind

        // Bind
        public void BindOnAdd(Action<TKey, TValue> listener)
        {
            OnAdd += listener;
        }

        public void BindOnRemove(Action<TKey> listener)
        {
            OnRemove += listener;
        }

        public void BindOnUpdate(Action<TKey, TValue> listener)
        {
            OnUpdate += listener;
        }

        public void BindOnClear(Action listener)
        {
            OnClear += listener;
        }

        public void BindOnChange(Action<Dictionary<TKey, TValue>> listener)
        {
            OnChange += listener;
        }

        // Unbind
        public void UnbindOnAdd(Action<TKey, TValue> listener)
        {
            OnAdd -= listener;
        }

        public void UnbindOnRemove(Action<TKey> listener)
        {
            OnRemove -= listener;
        }

        public void UnbindOnUpdate(Action<TKey, TValue> listener)
        {
            OnUpdate -= listener;
        }

        public void UnbindOnClear(Action listener)
        {
            OnClear -= listener;
        }

        public void UnbindOnChange(Action<Dictionary<TKey, TValue>> listener)
        {
            OnChange -= listener;
        }

        #endregion Bind

        public void Add(TKey key, TValue value)
        {
            if (_values.TryAdd(key, value))
            {
                OnAdd?.Invoke(key, value);
            }
        }

        public void Remove(TKey key)
        {
            if (_values.Remove(key))
            {
                OnRemove?.Invoke(key);
            }
        }

        public void Update(TKey key, TValue value)
        {
            if (!_values.ContainsKey(key))
            {
                Add(key, value);
                return;
            }

            _values[key] = value;
            OnUpdate?.Invoke(key, value);
        }

        public void Clear()
        {
            _values.Clear();
            OnClear?.Invoke();
        }

        public void Change(Dictionary<TKey, TValue> value)
        {
            _values = value;
            OnChange?.Invoke(value);
        }

        public bool ContainsKey(TKey key)
        {
            return _values.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return _values.ContainsValue(value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _values.TryGetValue(key, out value);
        }

        public bool TryAdd(TKey key, TValue value)
        {
            if (_values.TryAdd(key, value))
            {
                OnAdd?.Invoke(key, value);
                return true;
            }
            return false;
        }

        public TValue this[TKey key]
        {
            get => _values[key];
            set
            {
                if (_values.ContainsKey(key))
                {
                    Update(key, value);
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // void IReference.Clear()
        // {
        //     OnAdd = null;
        //     OnRemove = null;
        //     OnUpdate = null;
        //     OnClear = null;
        //     OnChange = null;
        //     _values.Clear();
        // }
    }

}