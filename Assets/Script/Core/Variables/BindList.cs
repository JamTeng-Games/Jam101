using System;
using System.Collections;
using System.Collections.Generic;

namespace Jam.Core
{

    public interface IListBind<T>
    {
        public void OnAdd(int index, T value);
        public void OnRemove(int index, T value);
        public void OnUpdate(int index, T value);
        public void OnChange(List<T> value);
        public void OnClear();
    }

    /// NOTE: Use BindListHelper class to bind list
    public class BindList<T> : IList<T>
    {
        private List<T> _values = new List<T>();

        private event Action<int, T> OnAdd;
        private event Action<int, T> OnRemove;
        private event Action<int, T> OnUpdate;
        private event Action<int, T> OnModify;
        private event Action OnClear;
        private event Action<List<T>> OnChange;

        public int Count => _values.Count;
        public bool IsReadOnly => false;

        public static BindList<T> Create()
        {
            BindList<T> list = new BindList<T>();// ReferencePool.Get<BindList<T>>();
            return list;
        }

        public void Dispose()
        {
            // ReferencePool.Release(this);
        }

        // Bind
        public void BindOnAdd(Action<int, T> listener)
        {
            OnAdd += listener;
        }

        public void BindOnRemove(Action<int, T> listener)
        {
            OnRemove += listener;
        }

        public void BindOnUpdate(Action<int, T> listener)
        {
            OnUpdate += listener;
        }

        public void BindOnModify(Action<int, T> listener)
        {
            OnModify += listener;
        }

        public void BindOnClear(Action listener)
        {
            OnClear += listener;
        }

        public void BindOnChange(Action<List<T>> listener)
        {
            OnChange += listener;
        }

        // Unbind
        public void UnbindOnAdd(Action<int, T> listener)
        {
            OnAdd -= listener;
        }

        public void UnbindOnRemove(Action<int, T> listener)
        {
            OnRemove -= listener;
        }

        public void UnbindOnUpdate(Action<int, T> listener)
        {
            OnUpdate -= listener;
        }

        public void UnbindOnModify(Action<int, T> listener)
        {
            OnModify -= listener;
        }

        public void UnbindOnClear(Action listener)
        {
            OnClear -= listener;
        }

        public void UnbindOnChange(Action<List<T>> listener)
        {
            OnChange -= listener;
        }

        public void Add(T value)
        {
            _values.Add(value);
            OnAdd?.Invoke(_values.Count - 1, value);
            OnModify?.Invoke(_values.Count - 1, value);
        }

        public bool Remove(T value)
        {
            int index = _values.IndexOf(value);
            if (index == -1)
                return false;

            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            T target = _values[index];
            _values.RemoveAt(index);
            OnRemove?.Invoke(index, target);
            OnModify?.Invoke(index, target);
        }

        public void Insert(int index, T value)
        {
            _values.Insert(index, value);
            OnAdd?.Invoke(index, value);
            OnModify?.Invoke(index, value);
        }

        public void Update(int index, T value)
        {
            _values[index] = value;
            OnUpdate?.Invoke(index, value);
            OnModify?.Invoke(index, value);
        }

        public void Clear()
        {
            _values.Clear();
            OnClear?.Invoke();
            OnModify?.Invoke(-1, default);
        }

        public int FindIndex(Predicate<T> match)
        {
            return _values.FindIndex(match);
        }

        public void Sort(Comparison<T> comparison = null)
        {
            if (comparison == null)
                _values.Sort();
            else
                _values.Sort(comparison);

            OnChange?.Invoke(_values);
            OnModify?.Invoke(-1, default);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
            OnChange?.Invoke(_values);
            OnModify?.Invoke(-1, default);
        }

        public int IndexOf(T value)
        {
            return _values.IndexOf(value);
        }

        public bool Contains(T value)
        {
            return _values.Contains(value);
        }

        public T this[int index]
        {
            get => _values[index];
            set => Update(index, value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _values.Count; i++)
                yield return _values[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
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