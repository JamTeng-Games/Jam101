using cfg;
using Jam.Core;
using Jam.Runtime.Event;
using UnityEngine.EventSystems;

namespace Jam.Runtime.UI_
{

    public partial class HomePanel
    {
        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _img_hero.AddEventListener(EventTriggerType.PointerUp, OnClickHeroImg);
            _btn_battle.onClick.AddListener(OnClickBattle);
            _btn_settings.onClick.AddListener(OnClickSettings);
        }

        public override void OnClose()
        {
            _img_hero.RemoveAllEventListeners();
            _btn_battle.onClick.RemoveListener(OnClickBattle);
            _btn_settings.onClick.RemoveListener(OnClickSettings);

            this.ClearWidgets();
        }

        protected override void OnTick(float dt)
        {
        }

        private void OnClickBattle()
        {
            G.Event.Send(GlobalEventId.EnterCombat);
        }

        private void OnClickSettings()
        {
        }

        private void OnClickHeroImg(BaseEventData obj)
        {
            G.UI.Open(UIPanelId.ChooseHero);
        }
    }

}