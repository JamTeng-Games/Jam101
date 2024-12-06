using System;

namespace Quantum.Graph.Skill
{

    [Serializable]
    public enum AttributeType
    {
        None,

        Hp,                       // 当前血量 int
        MaxHp,                    // 最大血量 int
        Attack,                   // 伤害     int
        Defence,                  // 护甲     int
        Speed,                    // 移动速度  int
        HpRecover,                // 生命恢复  int
        Shield,                   // 护盾     int
        AttackDistance,           // 攻击距离  int / 10
        AttackClipNum,            // 普通攻击弹夹数量
        AttackClipRecover,        // 普通弹夹恢复时间 int
        SkillClipNum,             // 技能弹夹数量 int
        SkillClipRecover,         // 技能CD int
        SkillDistance,            // 技能距离 int / 10
        CriticalChance,           // 暴击率, int / 100
        CriticalAttackMultiplier, // 暴击伤害, int / 100
        SuperSkillNeedPower,      // 大招所需能量条 int
        AttackHitAddSuperPower,   // 普通攻击命中增加大招能量 int
        SkillHitAddSuperPower,    // 技能命中增加大招能量 int

        __End__, // 终止占位符
    }

}