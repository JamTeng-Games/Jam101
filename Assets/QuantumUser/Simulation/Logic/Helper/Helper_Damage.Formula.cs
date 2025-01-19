using Photon.Deterministic;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    // 伤害计算
    public static unsafe partial class Helper_Damage
    {
        // 计算伤害的公式
        public static int Calculate(Frame f, in DamageInfo damageInfo)
        {
            bool isHeal = IsHeal(damageInfo);

            Helper_Attrib.TryGetAttribValue(f, damageInfo.source, AttributeType.Attack, out int attack);
            // 初始伤害
            int baseDamage = damageInfo.damage.aoe + damageInfo.damage.bullet + attack;

            // 闪避
            bool isDodge = !isHeal && CalcDodge(f, damageInfo);

            // 暴击
            bool isCritical = CalcCritical(f, damageInfo);

            // TODO: 暴击必定命中呢还是闪避了就不暴击了 ???
            // 暴击默认两倍伤害
            FP multiplier = FP._1;
            // 暴击
            if (isCritical)
            {
                Log.Debug("isCritical");
                multiplier = FP._2;
                if (Helper_Attrib.TryGetAttribValue(f, damageInfo.source, AttributeType.CriticalAttackMultiplier,
                                                    out int criticalAttackMultiplier))
                {
                    multiplier += criticalAttackMultiplier / FP._100;
                }
            }
            // 没暴击, 但是闪避了
            else if (isDodge)
            {
                Log.Debug("isDodge");
                multiplier = FP._0;
            }

            // 再加上分散度
            FP variation = CalcVariation(f, damageInfo.varianceRate);
            Log.Debug($"variation {variation}");

            // 最终伤害
            FP damageValue = (baseDamage * multiplier) * (FP._1 + variation);
            Log.Debug($"Damage {damageValue}");
            return FPMath.RoundToInt(damageValue);
        }

        // 计算闪避
        private static bool CalcDodge(Frame f, in DamageInfo damageInfo)
        {
            // 先算命中
            FP rand = f.RNG->Next(FP._0, FP._1);
            bool isMiss = rand < FP._1 - damageInfo.hitRate;
            if (isMiss)
                return isMiss;

            // 再算闪避
            if (!Helper_Attrib.TryGetAttribValue(f, damageInfo.target, AttributeType.DodgeRate, out int dodgeRate))
                return isMiss;
            rand = f.RNG->Next(FP._0, FP._1);
            FP dodgeRateFP = dodgeRate / FP._100;
            return rand < dodgeRateFP;
        }

        // 计算暴击
        private static bool CalcCritical(Frame f, in DamageInfo damageInfo)
        {
            FP rand = f.RNG->Next(FP._0, FP._1);
            bool isCritical = rand < damageInfo.criticalRate;
            return isCritical;
        }

        private static FP CalcVariation(Frame f, FP variation)
        {
            FP rand = f.RNG->Next(-variation, variation);
            return rand;
        }
    }

}