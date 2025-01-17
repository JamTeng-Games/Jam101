using Photon.Deterministic;

namespace Quantum.Helper
{

    public static unsafe partial class Helper_Damage
    {
        public static void DoDamage(Frame f,
                                    EntityRef source,
                                    EntityRef target,
                                    EDamageInfoType damageType,
                                    Damage damage,
                                    FP hitRate,
                                    FP criticalRate,
                                    FP varianceRate,
                                    FP angle)
        {
            DamageInfo damageInfo = new DamageInfo
            {
                source = source,
                target = target,
                damageType = damageType,
                damage = damage,
                hitRate = hitRate,
                criticalRate = criticalRate,
                varianceRate = varianceRate,
                angle = angle
            };
            if (f.Unsafe.TryGetPointerSingleton<SDamageInfoComp>(out var dmgComp))
            {
                var dmgInfos = f.ResolveList(dmgComp->DamageInfos);
                dmgInfos.Add(damageInfo);
            }
        }

        public static void DoDamage(Frame f, DamageInfo damageInfo)
        {
            if (f.Unsafe.TryGetPointerSingleton<SDamageInfoComp>(out var dmgComp))
            {
                var dmgInfos = f.ResolveList(dmgComp->DamageInfos);
                dmgInfos.Add(damageInfo);
            }
        }

        // 是否会被杀死
        public static bool CanBeKilled(Frame f, EntityRef entity, in DamageInfo damageInfo)
        {
            if (!f.TryGet<StatsComp>(entity, out var statsComp))
                return false;
            if (statsComp.IsImmune > 0 || IsHeal(damageInfo))
                return false;
            int damageValue = Calculate(f, damageInfo);
            return damageValue >= statsComp.Hp;
        }

        #region DamageInfo

        public static bool IsHeal(in DamageInfo damageInfo)
        {
            return damageInfo.damageType.IsFlagSet(EDamageInfoType.DirectHeal) ||
                   damageInfo.damageType.IsFlagSet(EDamageInfoType.PeriodHeal);
        }

        public static bool IsDirectDamage(in DamageInfo damageInfo)
        {
            return damageInfo.damageType.IsFlagSet(EDamageInfoType.DirectDamage);
        }

        public static void AddBuffToInfo(Frame f, ref DamageInfo damageInfo, AddBuffInfo addBuffInfo)
        {
            if (!f.TryResolveList(damageInfo.addBuffs, out var addBuffs))
            {
                addBuffs = f.AllocateList<AddBuffInfo>();
            }
            addBuffs.Add(addBuffInfo);
        }

        public static void DestroyDamageInfo(Frame f, in DamageInfo damageInfo)
        {
            f.TryFreeList(damageInfo.addBuffs);
        }

        #endregion
    }

}