using Jam.Cfg;

namespace Jam.Runtime.UI_
{

    public class UIWidgetAttribute : System.Attribute
    {
        public readonly UIWidgetId Id;

        public UIWidgetAttribute(UIWidgetId id)
        {
            Id = id;
        }
    }

}