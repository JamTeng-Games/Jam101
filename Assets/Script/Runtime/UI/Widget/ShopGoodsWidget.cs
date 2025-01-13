using Jam.Core;
using Jam.Runtime.Constant;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;
using Jam.Runtime.Net_;
using UnityEngine;

namespace Jam.Runtime.UI_
{

    public partial class ShopGoodsWidget
    {
        private ShopGoods _data;

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_buy.onClick.AddListener(OnClickBuy);
            Refresh(userData as ShopGoods);
        }

        public override void OnClose()
        {
            _btn_buy.onClick.RemoveListener(OnClickBuy);
            _data = null;
        }

        protected override void OnTick(float dt)
        {
        }

        public void Refresh(ShopGoods newData)
        {
            _data = newData;

            int itemCfgId = _data.item_id;
            Cfg.Item itemCfg = G.Cfg.TbItem[itemCfgId];
            // img
            {
                G.Asset.Load(AssetPath.ItemIcon(itemCfg.Icon), typeof(Sprite), assetWrap =>
                {
                    _img_item_icon.sprite = (Sprite)assetWrap.Asset;
                }, null);
            }
            // quality
            {
                var colorType = itemCfg.Quality.ToString();
                var colorHex = G.Cfg.TbColor[colorType].Value;
                _img_quality.color = Utils.Color.FromHex(colorHex);
            }
            // name
            {
                _txt_name.text = itemCfg.Name;
            }
            // type
            {
                _txt_type.text = itemCfg.Type.ToString();
            }
            // price
            {
                _txt_price.text = _data.price.ToString();
            }
            // desc
            {
                _txt_desc.text = itemCfg.Desc;
            }
            // sold out
            {
                _node_sold.gameObject.SetActive(_data.amount == 0);
            }
        }

        private void OnClickBuy()
        {
            if (_data == null)
                return;

            int itemCfgId = _data.item_id;
            Cfg.Item itemCfg = G.Cfg.TbItem[itemCfgId];

            // check money
            if (!MoneyBagHelper.HasMoney(itemCfg.MoneyId, _data.price))
            {
                JLog.Debug($"Not enough money: {itemCfg.MoneyId} {_data.price}");
                return;
            }

            G.Net.Send(NetCmd.CS_BuyGoods, new BuyGoodsReq() { id = _data.id, amount = _data.amount, });
        }
    }

}