using UnityEngine;

namespace J.Core
{
    public static class GameObjectEx
    {
        public static GameObject FindInactive(string name)
        {
            foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.name == name)
                    return go;
            }
            return null;
        }

        public static TComp GetOrAddComponent<TComp>(this GameObject obj) where TComp : Component
        {
            TComp comp = obj.GetComponent<TComp>();
            if (comp == null)
            {
                comp = obj.AddComponent<TComp>();
            }
            return comp;
        }
    }

}