using System;
using System.Collections.Generic;

namespace Jam.Runtime.Data_
{

    // 角色数据
    [Serializable]
    public class UserData : IData
    {
        public string rid;                                 // record id, 数据库中的记录id
        public string name = string.Empty;                 // 玩家名称
        public int hero;                                   // 英雄配置id
        public ItemBagData itemBag = new ItemBagData();    // 背包物品
        public MoneyBagData moneyBag = new MoneyBagData(); // 背包货币
        public ShopData shopData = new ShopData();         // 商店数据
        public int round;

        public void UpdateData(UserData newData)
        {
        }

        protected override void OnDispose()
        {
        }
    }

}