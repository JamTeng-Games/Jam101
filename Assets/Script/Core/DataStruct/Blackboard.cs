using System.Collections.Generic;

namespace Jam.Core
{

    public class Blackboard : IReference
    {
        private Dictionary<string, int> _intValues = new Dictionary<string, int>();
        private Dictionary<string, float> _floatValues = new Dictionary<string, float>();
        private Dictionary<string, string> _stringValues = new Dictionary<string, string>();
        private Dictionary<string, object> _objectValues = new Dictionary<string, object>();

        public static Blackboard Create()
        {
            return ReferencePool.Get<Blackboard>();
        }

        public void Dispose()
        {
            ReferencePool.Release(this);
        }

        void IReference.Clean()
        {
            _intValues.Clear();
            _floatValues.Clear();
            _stringValues.Clear();
            _objectValues.Clear();
        }

        public bool TryGetInt(string key, out int value)
        {
            return _intValues.TryGetValue(key, out value);
        }

        public bool TryGetFloat(string key, out float value)
        {
            return _floatValues.TryGetValue(key, out value);
        }

        public bool TryGetString(string key, out string value)
        {
            return _stringValues.TryGetValue(key, out value);
        }

        public bool TryGetObject(string key, out object value)
        {
            return _objectValues.TryGetValue(key, out value);
        }

        public int GetInt(string key)
        {
            return _intValues[key];
        }

        public float GetFloat(string key)
        {
            return _floatValues[key];
        }

        public string GetString(string key)
        {
            return _stringValues[key];
        }

        public object GetObject(string key)
        {
            return _objectValues[key];
        }

        public void SetInt(string key, int value)
        {
            _intValues[key] = value;
        }

        public void SetFloat(string key, float value)
        {
            _floatValues[key] = value;
        }

        public void SetString(string key, string value)
        {
            _stringValues[key] = value;
        }

        public void SetObject(string key, object value)
        {
            _objectValues[key] = value;
        }

        public void ClearInt(string key, int value)
        {
            _intValues.Remove(key);
        }

        public void ClearFloat(string key, float value)
        {
            _floatValues.Remove(key);
        }

        public void ClearString(string key, string value)
        {
            _stringValues.Remove(key);
        }

        public void ClearObject(string key, object value)
        {
            _objectValues.Remove(key);
        }

        public void ClearAll()
        {
            _intValues.Clear();
            _floatValues.Clear();
            _stringValues.Clear();
            _objectValues.Clear();
        }
    }

}