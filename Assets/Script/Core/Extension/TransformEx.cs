using UnityEngine;

namespace J.Core
{

    public static class TransformEx
    {
        public static Transform FindRecursively(this Transform parent, string name)
        {
            if (parent.name == name)
            {
                return parent;
            }
            for (int i = 0; i < parent.childCount; ++i)
            {
                var child = parent.GetChild(i);
                var res = FindRecursively(child, name);
                if (res != null)
                {
                    return res;
                }
            }

            return null;
        }

        public static void AddLocalPositionX(this Transform transform, float x)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x + x, localPosition.y, localPosition.z);
            transform.localPosition = localPosition;
        }

        public static void AddLocalPositionY(this Transform transform, float y)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y + y, localPosition.z);
            transform.localPosition = localPosition;
        }

        public static void AddLocalPositionZ(this Transform transform, float z)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z + z);
            transform.localPosition = localPosition;
        }

        public static void AddLocalPositionXY(this Transform transform, float x, float y)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x + x, localPosition.y + y, localPosition.z);
            transform.localPosition = localPosition;
        }

        public static void AddLocalPositionXZ(this Transform transform, float x, float z)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x + x, localPosition.y, localPosition.z + z);
            transform.localPosition = localPosition;
        }

        public static void AddLocalPositionYZ(this Transform transform, float y, float z)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y + y, localPosition.z + z);
            transform.localPosition = localPosition;
        }

        public static void AddLocalPositionXYZ(this Transform transform, float x, float y, float z)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x + x, localPosition.y + y, localPosition.z + z);
            transform.localPosition = localPosition;
        }

        public static void SetLocalPositionXY(this Transform transform, float x, float y)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(x, y, localPosition.z);
            transform.localPosition = localPosition;
        }

        public static bool IsInFront(this Transform a, Transform b)//a是否在b的前面
        {
            return Vector3.Dot(b.forward, a.position - b.position) > 0;
        }
    }

}