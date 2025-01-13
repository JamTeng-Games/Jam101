using System.Collections.Generic;
using Jam.Cfg;
using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;
using Jam.Runtime.Helpers;
using Jam.Runtime.Net_;

namespace Jam.Runtime.UI_
{

    public partial class ShopPanel
    {
        private int _moneyWidgetId = 0;
        private List<int> _goodsWidgetIds;
        private List<int> _inventoryWidgetIds;

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _moneyWidgetId = this.AddWidget(UIWidgetId.Money, _node_money);
            _btn_back.onClick.AddListener(OnClickBack);
            _btn_refresh.onClick.AddListener(OnClickRefresh);
            _btn_stats.onClick.AddListener(OnClickStats);
            _btn_inventory.onClick.AddListener(OnClickInventory);
            // _btn_unfold_player_item.onClick.AddListener(OnClickUnfoldPlayerItems);
            G.Event.Subscribe(GlobalEventId.ShopRefresh, OnShopRefresh);
            G.Event.Subscribe<int, int>(GlobalEventId.ShopGoodsUpdate, OnShopGoodsUpdate);
            G.Event.Subscribe<int>(GlobalEventId.ShopGoodsRemove, OnShopGoodsRemove);
            G.Event.Subscribe(GlobalEventId.ItemAnyUpdate, OnAnyItemUpdate);
            G.Event.Subscribe(GlobalEventId.RoundUpdate, OnRoundUpdate);
            Refresh();
        }

        public override void OnClose()
        {
            _btn_back.onClick.RemoveListener(OnClickBack);
            _btn_refresh.onClick.RemoveListener(OnClickRefresh);
            _btn_stats.onClick.RemoveListener(OnClickStats);
            _btn_inventory.onClick.RemoveListener(OnClickInventory);
            G.Event.Unsubscribe(GlobalEventId.ShopRefresh, OnShopRefresh);
            G.Event.Unsubscribe<int, int>(GlobalEventId.ShopGoodsUpdate, OnShopGoodsUpdate);
            G.Event.Unsubscribe<int>(GlobalEventId.ShopGoodsRemove, OnShopGoodsRemove);
            G.Event.Unsubscribe(GlobalEventId.ItemAnyUpdate, OnAnyItemUpdate);
            G.Event.Unsubscribe(GlobalEventId.RoundUpdate, OnRoundUpdate);
            this.ClearWidgets();
            _goodsWidgetIds.Clear();
            _inventoryWidgetIds.Clear();
            _moneyWidgetId = 0;
        }

        protected override void OnTick(float dt)
        {
        }

        private void Refresh()
        {
            // title
            _txt_level.text = $"Round {G.Data.UserData.round.ToString()}";
            // goods
            RefreshGoods();
            // inventory
            RefreshInventory();
            //refresh money
            {
                var refreshMoney = G.Data.UserData.shopData.refresh_money;
                _txt_refresh_price.text = refreshMoney.price.ToString();
            }
        }

        private void RefreshGoods()
        {
            // var allGoods = G.Data.UserData.shopData.goods;
            // if (_goodsWidgetIds == null)
            //     _goodsWidgetIds = new List<int>(allGoods.Count);
            //
            // var goods = allGoods[0];
            // var widgetId = this.AddWidget(UIWidgetId.ShopGoods, _node_goods_list, userData: goods);

            var allGoods = G.Data.UserData.shopData.goods;
            if (_goodsWidgetIds == null)
                _goodsWidgetIds = new List<int>(allGoods.Count);

            // 隐藏多余的
            if (_goodsWidgetIds.Count > allGoods.Count)
            {
                for (int i = allGoods.Count; i < _goodsWidgetIds.Count; i++)
                {
                    var widget = this.GetWidget<ShopGoodsWidget>(_goodsWidgetIds[i]);
                    widget.gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < allGoods.Count; i++)
            {
                var goods = allGoods[i];
                if (_goodsWidgetIds.Count <= i)
                {
                    var widgetId = this.AddWidget(UIWidgetId.ShopGoods, _node_goods_list, userData: goods);
                    _goodsWidgetIds.Add(widgetId);
                }
                else
                {
                    var widget = this.GetWidget<ShopGoodsWidget>(_goodsWidgetIds[i]);
                    widget?.gameObject.SetActive(true);
                    widget?.Refresh(goods);
                }
            }
        }

        private void RefreshInventory()
        {
            var itemsInBag = G.Data.UserData.itemBag.item_bag;
            if (_inventoryWidgetIds == null)
                _inventoryWidgetIds = new List<int>(itemsInBag.Count);

            // 隐藏多余的
            if (_inventoryWidgetIds.Count > itemsInBag.Count)
            {
                for (int i = itemsInBag.Count; i < _inventoryWidgetIds.Count; i++)
                {
                    var widget = this.GetWidget<ItemCellWidget>(_inventoryWidgetIds[i]);
                    widget.gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < itemsInBag.Count; i++)
            {
                var bagItem = itemsInBag[i];
                if (_inventoryWidgetIds.Count <= i)
                {
                    var widgetId = this.AddWidget(UIWidgetId.ItemCell, _node_inventory, w =>
                    {
                        (w as ItemCellWidget).DisableSelect();
                    }, userData: bagItem);
                    JLog.Debug($"AddWidget ItemCell {widgetId}");
                    _inventoryWidgetIds.Add(widgetId);
                }
                else
                {
                    var widget = this.GetWidget<ItemCellWidget>(_inventoryWidgetIds[i]);
                    widget?.gameObject.SetActive(true);
                    widget?.Refresh(bagItem);
                    widget?.DisableSelect();
                }
            }
        }

        private void OnShopGoodsRemove(int itemId)
        {
            RefreshGoods();
        }

        private void OnShopGoodsUpdate(int itemId, int amount)
        {
            RefreshGoods();
        }

        private void OnShopRefresh()
        {
            RefreshGoods();
            //refresh money
            {
                var refreshMoney = G.Data.UserData.shopData.refresh_money;
                _txt_refresh_price.text = refreshMoney.price.ToString();
            }
        }

        private void OnAnyItemUpdate()
        {
            RefreshInventory();
        }

        private void OnRoundUpdate()
        {
            _txt_level.text = $"Round {G.Data.UserData.round.ToString()}";
        }

        private void OnClickBack()
        {
            G.UI.Back();
        }

        private void OnClickRefresh()
        {
            // Check money
            int moneyId = G.Data.UserData.shopData.refresh_money.money_id;
            int price = G.Data.UserData.shopData.refresh_money.price;
            if (!MoneyBagHelper.HasMoney(moneyId, price))
            {
                JLog.Debug("Not enough money");
                return;
            }
            G.Net.Send(NetCmd.CS_RefreshShop, EmptyMsg.Data);
        }

        private void OnClickStats()
        {
            JLog.Debug("Click stats");
        }

        private void OnClickInventory()
        {
            G.UI.Open(UIPanelId.Inventory);
        }
    }

}