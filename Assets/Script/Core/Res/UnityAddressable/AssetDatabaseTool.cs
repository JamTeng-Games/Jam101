using UnityEngine;

namespace J.Core
{
    public class AssetDatabaseTool
    {
        public T Load<T>(string path) where T : Object
        {
#if UNITY_EDITOR
            T res = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            return res;
#else
            return null;
#endif
        }

        public void Unload(string path)
        {
        }
    }
}