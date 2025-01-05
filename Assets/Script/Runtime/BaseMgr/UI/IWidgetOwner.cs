using System.Collections.Generic;
using cfg;
using UnityEngine;

namespace Jam.Runtime.UI_
{

    public interface IWidgetOwner
    {
        public Dictionary<int, UIWidget> Widgets { get; }
    }

    public static class IWidgetContainerEx
    {
        public static int AddWidget(this IWidgetOwner owner,
                                    UIWidgetId widgetType,
                                    Transform parent,
                                    System.Action<UIWidget> callback = null,
                                    object userData = null)
        {
            return G.UI.CreateWidget(widgetType, owner, parent, w =>
            {
                owner.Widgets.Add(w.Id, w);
                callback?.Invoke(w);
            }, userData);
        }

        public static void RemoveWidget(this IWidgetOwner owner, int id)
        {
            owner.Widgets.Remove(id);
            G.UI.DestroyWidget(id);
        }

        public static T GetWidget<T>(this IWidgetOwner owner, int id) where T : UIWidget
        {
            if (owner.Widgets.TryGetValue(id, out var widget))
                return widget as T;
            return null;
            // return G.UI.GetWidget<T>(id);
        }
        
        public static void ClearWidgets(this IWidgetOwner owner)
        {
            foreach (var widget in owner.Widgets.Values)
            {
                G.UI.DestroyWidget(widget.Id);
            }
            owner.Widgets.Clear();
        }
    }

}