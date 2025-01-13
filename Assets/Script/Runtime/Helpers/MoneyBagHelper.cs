using System.Collections.Generic;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;

namespace Jam.Runtime.Helpers
{

    public static class MoneyBagHelper
    {
        public static MoneyBagData GetMoneys() => G.Data.UserData.moneyBag;

        public static void OnAddMoney(int id, int amount)
        {
            var money = GetMoneyById(id);
            if (money != null)
            {
                money.amount = amount;
            }
            else
            {
                GetMoneys().money_bag.Add(new BagMoney { id = id, amount = amount });
            }
            G.Event.Send(GlobalEventId.MoneyAdd, id, amount);
        }

        public static void OnCostMoney(int id, int amount)
        {
            var money = GetMoneyById(id);
            if (money != null)
            {
                money.amount = amount;
                if (money.amount <= 0)
                {
                    GetMoneys().money_bag.Remove(money);
                }
                G.Event.Send(GlobalEventId.MoneyCost, id, amount);
            }
        }

        public static void OnUpdateMoneys(MoneyBagData data)
        {
            var moneys = GetMoneys();
            moneys.money_bag?.Clear();
            moneys.money_bag = data.money_bag;
            G.Event.Send(GlobalEventId.MoneyUpdateAll);
        }

        public static bool HasMoney(int id, int amount)
        {
            var money = GetMoneyById(id);
            return money != null && money.amount >= amount;
        }

        public static BagMoney GetMoneyById(int id)
        {
            var moneys = GetMoneys();
            if (moneys.money_bag == null)
                return null;
            foreach (var money in moneys.money_bag)
            {
                if (money.id == id)
                {
                    return money;
                }
            }
            return null;
        }
    }

}