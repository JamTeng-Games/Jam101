using UnityEngine.UIElements;

namespace J.Core
{

    public static class VisualElementEx
    {
        public static bool TryRemove(this VisualElement ve, VisualElement toRemove)
        {
            if (toRemove == null)
                return false;
            if (ve.Contains(toRemove))
            {
                toRemove.RemoveFromHierarchy();
                return true;
            }
            return false;
        }
    }

}