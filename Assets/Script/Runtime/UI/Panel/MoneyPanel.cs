using cfg;
using Jam.Runtime.Event;

namespace Jam.Runtime.UI_
{

    public partial class MoneyPanel
    {
        // 当这些界面打开时，隐藏自己
        private UIPanelId[] _toHidePanels = new UIPanelId[] { UIPanelId.Matching, };

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            G.Event.Subscribe<UIPanelId>(GlobalEventId.PanelOpen, OnPanelOpen);
            G.Event.Subscribe<UIPanelId>(GlobalEventId.PanelClosed, OnPanelClosed);
        }

        public override void OnClose()
        {
            G.Event.Unsubscribe<UIPanelId>(GlobalEventId.PanelOpen, OnPanelOpen);
            G.Event.Unsubscribe<UIPanelId>(GlobalEventId.PanelClosed, OnPanelClosed);
        }

        protected override void OnTick(float dt)
        {
        }

        private void OnPanelOpen(UIPanelId panelId)
        {
            if (IsPanelInHideList(panelId))
            {
                HideSelf();
            }
        }

        private void OnPanelClosed(UIPanelId panelId)
        {
            if (IsPanelInHideList(panelId))
            {
                ShowSelf();
            }
        }

        private bool IsPanelInHideList(UIPanelId panelId)
        {
            foreach (var panel in _toHidePanels)
            {
                if (panel == panelId)
                {
                    return true;
                }
            }
            return false;
        }
    }

}