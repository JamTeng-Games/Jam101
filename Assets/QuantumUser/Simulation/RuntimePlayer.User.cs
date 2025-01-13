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
            public ulong rid;        // record id, 数据库中的记录id
            public string name;      // 玩家名称
            public int level;        // 玩家等级

            public int round;       // 处于第几回合
            public int hero_id;     // 英雄配置id
            public List<int> items; // 背包物品
        }
    }

}