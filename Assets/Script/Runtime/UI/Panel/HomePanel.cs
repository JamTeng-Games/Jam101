using Jam.Runtime.Event;

namespace Jam.Runtime.UI_
{

    public partial class HomePanel
    {
        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_battle.onClick.AddListener(OnClickBattle);
        }

        public override void OnClose()
        {
            _btn_battle.onClick.RemoveListener(OnClickBattle);
        }

        protected override void OnTick(float dt)
        {
        }

        private void OnClickBattle()
        {
            G.Event.Send(GlobalEventId.EnterCombat);
        }
    }

}