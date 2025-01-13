using System.Collections.Generic;
using Jam.Cfg;
using Jam.Runtime.Data_;

namespace Jam.Runtime.UI_
{

    public partial class InventoryPanel
    {
        private List<int> _itemWidgetsIds;
        private int _itemInfoWidgetId;
        private int _selectItemCfgId;
        private int _moneyWidgetId;

        public override void OnInit()
        {
            _itemWidgetsIds = new List<int>(32);
            _itemInfoWidgetId = 0;
        }

        public override void OnOpen(object userData)
        {
            _moneyWidgetId = this.AddWidget(UIWidgetId.Money, _node_money);
            _btn_back.onClick.AddListener(OnClickBack);
            _btn_home.onClick.AddListener(OnClickHome);

            Refresh();
            RefreshItemInfo(0);
        }

        public override void OnClose()
        {
            _btn_back.onClick.RemoveListener(OnClickBack);
            _btn_home.onClick.RemoveListener(OnClickHome);
            this.ClearWidgets();
            _itemInfoWidgetId = 0;
            _moneyWidgetId = 0;
            _itemWidgetsIds.Clear();
        }

        private void Refresh()
        {
            var itemsInBag = G.Data.UserData.itemBag.item_bag;

            // 隐藏多余的
            if (_itemWidgetsIds.Count > itemsInBag.Count)
            {
                for (int i = itemsInBag.Count; i < _itemWidgetsIds.Count; i++)
                {
                    var widget = this.GetWidget<ItemCellWidget>(_itemWidgetsIds[i]);
                    widget.gameObject.SetActive(false);
                    widget.ClearClickEvent();
                }
            }

            for (int i = 0; i < itemsInBag.Count; i++)
            {
                var bagItem = itemsInBag[i];
                if (_itemWidgetsIds.Count <= i)
                {
                    var widgetId = this.AddWidget(Cfg.UIWidgetId.ItemCell, _node_content, w =>
                    {
                        (w as ItemCellWidget).SetClickEvent(OnSelectItemCell);
                    }, userData: bagItem);
                    _itemWidgetsIds.Add(widgetId);
                }
                else
                {
                    var widget = this.GetWidget<ItemCellWidget>(_itemWidgetsIds[i]);
                    widget?.gameObject.SetActive(true);
                    widget?.Refresh(bagItem);
                    widget?.SetClickEvent(OnSelectItemCell);
                }
            }
        }

        private void OnClickBack()
        {
            G.UI.Back();
        }

        private void OnClickHome()
        {
            G.UI.BackToHome();
        }

        private void OnSelectItemCell(ItemCellWidget widget)
        {
            foreach (var (id, w) in this._widgets)
            {
                if (w is ItemCellWidget itemCellWidget)
                {
                    itemCellWidget.Unselect();
                }
            }
            widget.Select();
            _selectItemCfgId = widget.Data.id;

            var data = widget.Data;
            int itemCfgId = data.id;
            RefreshItemInfo(itemCfgId);
        }

        private void RefreshItemInfo(int itemCfgId)
        {
            if (_itemInfoWidgetId == 0)
            {
                _itemInfoWidgetId = this.AddWidget(Cfg.UIWidgetId.ItemInfo, _node_iteminfo, userData: itemCfgId);
            }
            else
            {
                var widget = this.GetWidget<ItemInfoWidget>(_itemInfoWidgetId);
                widget?.Refresh(itemCfgId);
            }
        }
    }

}