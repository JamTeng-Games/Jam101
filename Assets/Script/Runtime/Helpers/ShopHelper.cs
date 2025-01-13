using System.Resources;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;

namespace Jam.Runtime.Helpers
{

    public static class ShopHelper
    {
        public static void Create(ShopData data)
        {
            Refresh(data);
        }

        public static void Refresh(ShopData data)
        {
            G.Data.UserData.shopData = data;
            G.Event.Send(GlobalEventId.ShopRefresh);
        }

        public static void UpdateGoods(ShopGoodsUpdate data)
        {
            var oldGoods = GetGoodsById(data.goods_id);
            oldGoods.amount = data.amount;
            if (oldGoods.amount == 0)
            {
                G.Event.Send(GlobalEventId.ShopGoodsRemove, data.goods_id);
            }
            else
            {
                G.Event.Send(GlobalEventId.ShopGoodsUpdate, data.goods_id, data.amount);
            }
        }

        public static ShopGoods GetGoodsById(int id)
        {
            var shopData = G.Data.UserData.shopData;
            foreach (var goods in shopData.goods)
            {
                if (goods.id == id)
                {
                    return goods;
                }
            }
            return null;
        }
    }

}