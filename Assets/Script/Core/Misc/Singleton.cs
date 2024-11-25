using System;
using UnityEngine;

namespace Jam.Core
{

    public abstract class Singleton<T> where T : class, new()
    {
        protected static readonly T _instance = new T();

        public static T Instance => _instance;
    }

    public abstract class LazySingleton<T> where T : class, new()
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new T();
                return s_instance;
            }
        }
    }

    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly object s_singletonLock = new object();
        private static T s_instance;
        public static bool IsApplicationQuitting { get; protected set; }

        static MonoSingleton()
        {
            IsApplicationQuitting = false;
        }

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    lock (s_singletonLock)
                    {
                        T[] singletonInstances = FindObjectsOfType(typeof(T)) as T[];

                        if (singletonInstances.Length == 0)
                        {
                            GameObject go = new GameObject(typeof(T).Name);
                            s_instance = go.AddComponent<T>();
                            return s_instance;
                        }

                        if (singletonInstances.Length > 1)
                        {
                            if (Application.isEditor)
                                JLog.Warning("MonoSingleton<T>.Instance: Only 1 singleton instance can exist in the scene.");
                        }
                        s_instance = singletonInstances[0];
                    }
                }

                return s_instance;
            }
        }

        private void OnApplicationQuit()
        {
            IsApplicationQuitting = true;
        }
    }

}