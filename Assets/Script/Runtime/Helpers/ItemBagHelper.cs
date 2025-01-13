using System.Collections.Generic;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;

namespace Jam.Runtime.Helpers
{

    public static class ItemBagHelper
    {
        public static ItemBagData GetItems() => G.Data.UserData.itemBag;

        public static void OnAddItem(int id, int amount)
        {
            var bagItem = GetItemById(id);
            if (bagItem != null)
            {
                bagItem.amount += amount;
            }
            else
            {
                GetItems().item_bag.Add(new BagItem { id = id, amount = amount });
            }
            G.Event.Send(GlobalEventId.ItemAdd, id, amount);
            G.Event.Send(GlobalEventId.ItemAnyUpdate);
        }

        public static void OnRemoveItem(int id, int amount)
        {
            var money = GetItemById(id);
            if (money != null)
            {
                money.amount -= amount;
                if (money.amount <= 0)
                {
                    GetItems().item_bag.Remove(money);
                }
                G.Event.Send(GlobalEventId.ItemRemove, id, amount);
                G.Event.Send(GlobalEventId.ItemAnyUpdate);
            }
        }

        public static void OnUpdateItems(ItemBagData data)
        {
            var moneys = GetItems();
            moneys.item_bag?.Clear();
            moneys.item_bag = data.item_bag;
            G.Event.Send(GlobalEventId.ItemUpdateAll);
            G.Event.Send(GlobalEventId.ItemAnyUpdate);
        }

        public static BagItem GetItemById(int id)
        {
            var items = GetItems();
            foreach (var item in items.item_bag)
            {
                if (item.id == id)
                {
                    return item;
                }
            }
            return null;
        }
    }

}