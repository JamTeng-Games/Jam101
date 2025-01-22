using Jam.Cfg;
using Jam.Core;
using Jam.Runtime.Constant;
using Jam.Runtime.Event;
using Jam.Runtime.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Jam.Runtime.UI_
{

    public partial class HomePanel
    {
        private int _moneyWidgetId = 0;

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _moneyWidgetId = this.AddWidget(UIWidgetId.Money, _node_money);
            _img_hero.AddEventListener(EventTriggerType.PointerUp, OnClickHeroImg);
            _btn_battle.onClick.AddListener(OnClickBattle);
            _btn_shop.onClick.AddListener(OnClickShop);
            _btn_settings.onClick.AddListener(OnClickSettings);
            _btn_inventory.onClick.AddListener(OnClickInventory);
            G.Event.Subscribe(GlobalEventId.RoleDataUpdate, OnRoleDataUpdate);
            G.Event.Subscribe(GlobalEventId.RoundUpdate, OnRoundUpdate);
            G.Event.Subscribe<int>(GlobalEventId.HeroChange, OnHeroChange);
        }

        public override void OnClose()
        {
            _img_hero.RemoveAllEventListeners();
            _btn_battle.onClick.RemoveListener(OnClickBattle);
            _btn_shop.onClick.RemoveListener(OnClickShop);
            _btn_settings.onClick.RemoveListener(OnClickSettings);
            _btn_inventory.onClick.RemoveListener(OnClickInventory);
            G.Event.Unsubscribe(GlobalEventId.RoleDataUpdate, OnRoleDataUpdate);
            G.Event.Unsubscribe(GlobalEventId.RoundUpdate, OnRoundUpdate);
            G.Event.Unsubscribe<int>(GlobalEventId.HeroChange, OnHeroChange);

            _moneyWidgetId = 0;
            this.ClearWidgets();
        }

        public override void OnShow()
        {
            _txt_player_name.text = G.Data.UserData.name;
            _txt_round.text = $"Round {G.Data.UserData.round.ToString()}";
            RefreshHero();
        }

        private void OnRoleDataUpdate()
        {
            _txt_player_name.text = G.Data.UserData.name;
            RefreshHero();
        }

        private void OnRoundUpdate()
        {
            _txt_round.text = $"Round {G.Data.UserData.round.ToString()}";
        }

        private void OnHeroChange(int heroId)
        {
            Cfg.Hero heroCfg = G.Cfg.TbHero[heroId];
            G.Asset.Load(AssetPath.HeroIcon(heroCfg.HeadIcon), typeof(Sprite), assetWrap =>
            {
                _img_hero.sprite = (Sprite)assetWrap.Asset;
            }, null);
        }

        private void RefreshHero()
        {
            if (G.Data.UserData.hero == 0)
                return;
            OnHeroChange(G.Data.UserData.hero);
        }

        private void OnClickBattle()
        {
            if (RoomHelper.IsInRoom())
            {
                G.UI.Open(UIPanelId.Room);
            }
            else
            {
                G.UI.Open(UIPanelId.RoomList);
            }
            // G.Event.Send(GlobalEventId.EnterCombat);
        }

        private void OnClickShop()
        {
            G.UI.Open(UIPanelId.Shop);
        }

        private void OnClickInventory()
        {
            G.UI.Open(UIPanelId.Inventory);
        }

        private void OnClickSettings()
        {
            G.UI.Open(UIPanelId.Setting);
        }

        private void OnClickHeroImg(BaseEventData obj)
        {
            G.UI.Open(UIPanelId.HeroList);
        }
    }

}