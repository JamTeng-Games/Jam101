using System;
using System.Collections.Generic;

namespace Quantum
{

    public partial class RuntimePlayer
    {
        public HeroData heroData;

        // 角色数据
        [Serializable]
        public class HeroData
        {
            public string rid;          // record id, 数据库中的记录id
            public string name;         // 玩家名称
            public int hero;            // 英雄配置id
            public List<BagItem> items; // 背包物品
        }

        [Serializable]
        public class BagItem
        {
            public int id;
            public int amount;
        }
    }

}