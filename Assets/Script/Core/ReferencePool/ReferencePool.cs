using System;
using System.Collections.Generic;

namespace Jam.Core
{

    public static class ReferencePool
    {
        public static bool EnableStrictCheck = false;
        private static readonly Dictionary<Type, ReferenceCollection> _referenceCollections =
            new Dictionary<Type, ReferenceCollection>();

        public static T Get<T>() where T : class, IReference, new()
        {
            return GetReferenceCollection(typeof(T)).Get<T>();
        }

        public static IReference Get(Type referenceType)
        {
            return GetReferenceCollection(referenceType).Get();
        }

        public static void Release(IReference reference)
        {
            if (reference == null)
            {
                throw new Exception("Reference is invalid.");
            }

            Type referenceType = reference.GetType();
            GetReferenceCollection(referenceType).Release(reference);
        }

        public static void Add<T>(int count) where T : class, IReference, new()
        {
            GetReferenceCollection(typeof(T)).Add<T>(count);
        }

        public static void Add(Type referenceType, int count)
        {
            GetReferenceCollection(referenceType).Add(count);
        }

        public static void Remove<T>(int count) where T : class, IReference
        {
            GetReferenceCollection(typeof(T)).Remove(count);
        }

        public static void Remove(Type referenceType, int count)
        {
            GetReferenceCollection(referenceType).Remove(count);
        }

        public static void RemoveAll<T>() where T : class, IReference
        {
            GetReferenceCollection(typeof(T)).RemoveAll();
        }

        public static void RemoveAll(Type referenceType)
        {
            GetReferenceCollection(referenceType).RemoveAll();
        }

        public static void ClearAll()
        {
            foreach (KeyValuePair<Type, ReferenceCollection> referenceCollection in _referenceCollections)
            {
                referenceCollection.Value.RemoveAll();
            }

            _referenceCollections.Clear();
        }

        private static ReferenceCollection GetReferenceCollection(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new Exception("ReferenceType is invalid.");
            }

            ReferenceCollection referenceCollection = null;
            if (!_referenceCollections.TryGetValue(referenceType, out referenceCollection))
            {
                referenceCollection = new ReferenceCollection(referenceType);
                _referenceCollections.Add(referenceType, referenceCollection);
            }
            return referenceCollection;
        }
    }

}