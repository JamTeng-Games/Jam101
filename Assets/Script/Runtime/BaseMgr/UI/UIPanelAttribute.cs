using cfg;

namespace Jam.Runtime.UI_
{

    public class UIPanelAttribute : System.Attribute
    {
        public readonly UIPanelId Id;

        public UIPanelAttribute(UIPanelId id)
        {
            Id = id;
        }
    }

}