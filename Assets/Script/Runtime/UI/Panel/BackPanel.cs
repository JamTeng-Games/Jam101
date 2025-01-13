using Jam.Cfg;
using Jam.Runtime.Event;

namespace Jam.Runtime.UI_
{

    public partial class BackPanel
    {
        // 当这些界面打开时，隐藏自己
        private UIPanelId[] _toHidePanels = new UIPanelId[] { UIPanelId.Matching, UIPanelId.Home };

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_back.onClick.AddListener(OnClickBack);
            G.Event.Subscribe<UIPanelId>(GlobalEventId.PanelOpen, RefreshVisibility);
            G.Event.Subscribe<UIPanelId>(GlobalEventId.PanelClosed, RefreshVisibility);
        }

        public override void OnClose()
        {
            _btn_back.onClick.RemoveListener(OnClickBack);
            G.Event.Unsubscribe<UIPanelId>(GlobalEventId.PanelOpen, RefreshVisibility);
            G.Event.Unsubscribe<UIPanelId>(GlobalEventId.PanelClosed, RefreshVisibility);
        }

        protected override void OnTick(float dt)
        {
        }

        private void OnClickBack()
        {
            G.UI.Back();
        }

        private void RefreshVisibility(UIPanelId panelId)
        {
            var panels = G.UI.Panels;
            foreach (var p in panels)
            {
                
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