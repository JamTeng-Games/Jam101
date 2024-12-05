using System;

namespace Quantum.Graph.Skill
{

    [Serializable]
    public enum AttributeType
    {
        None,
        MoveSpeed, // 移动速度
        Hp,        // 当前血量
        MaxHp,     // 最大血量
//    HpRecover, // 生命恢复
//    HpSteal, // 生命窃取, 百分比
//    Shield, // 护盾
//    Defence, // 护甲
//    BreakDefence, // 穿甲
        Attack, // 伤害
//    AttackAddPercent, // 伤害加成, 百分比
        AttackDistance,    // 攻击距离
        CriticalRate,      // 暴击率, 百分比
        CriticalAttack,    // 暴击伤害, 百分比
        AttackClipNum,     // 普通攻击弹夹数量
        AttackClipRecover, // 普通弹夹恢复时间
        // 攻击充能恢复速度, 百分比
        SkillClip, // 技能弹夹数
        // 技能充能
        SkillClipRecover, // 技能CD减免, 百分比
        // 大招能量条
        // 减少大招能量条, 百分比
    }
}