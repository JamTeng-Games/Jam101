using System;

namespace Quantum.Graph.Skill
{

    [Serializable]
    public enum AttributeType
    {
        None,

        MaxHp,                    // 最大血量 int
        MaxMp,                    // 最大魔法 int
        Attack,                   // 伤害     int
        Defence,                  // 护甲     int
        Speed,                    // 移动速度  int 10s移动多远
        HpRecover,                // 生命恢复  int
        Shield,                   // 护盾     int
        AttackDistance,           // 普通攻击距离  int / 10
        AttackClipMaxNum,         // 普通攻击弹夹数量
        AttackClipRecover,        // 普通弹夹恢复时间 int
        SkillClipMaxNum,          // 技能弹夹数量 int
        SkillClipRecover,         // 技能弹夹恢复速度 int
        CriticalChance,           // 暴击率, int / 100
        CriticalAttackMultiplier, // 暴击伤害, int / 100
        SuperSkillNeedPower,      // 大招所需能量条 int
        AttackHitAddSuperPower,   // 普通攻击命中增加大招能量 int
        SkillHitAddSuperPower,    // 技能命中增加大招能量 int

        __End__, // 终止占位符
    }

}