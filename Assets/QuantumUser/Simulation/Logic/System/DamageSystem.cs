using System;
using System.Runtime.InteropServices;
using Quantum.Collections;
using Quantum.Graph.Skill;
using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class DamageSystem : SystemMainThread
    {
        public override void OnInit(Frame f)
        {
            f.GetOrAddSingleton<SDamageInfoComp>();
        }

        public override void Update(Frame f)
        {
            if (!f.Unsafe.TryGetPointerSingleton<SDamageInfoComp>(out var dmgComp))
                return;

            var dmgInfos = f.ResolveList(dmgComp->DamageInfos);
            Log.Debug("DamageInfos Count: " + dmgInfos.Count);

            for (int i = 0; i < dmgInfos.Count; i++)
            {
                var dmgInfo = dmgInfos[i];
                var sourceEntity = dmgInfo.source;
                var targetEntity = dmgInfo.target;

                if (!f.Exists(targetEntity))
                    continue;

                if (!f.TryGet<StatsComp>(targetEntity, out var targetStats))
                    continue;

                // Check if target is dead
                if (targetStats.Hp == 0)
                    continue;

                // 先执行攻击者的 Buff OnHit
                QList<BuffObj>? sourceBuffs = null;
                if (f.TryGet<BuffComp>(sourceEntity, out var sourceBuffComp))
                {
                    sourceBuffs = f.ResolveList(sourceBuffComp.Buffs);
                    for (int j = 0; j < sourceBuffs.Value.Count; j++)
                    {
                        var buffObj = sourceBuffs.Value[j];
                        Helper_Buff.OnHit(f, sourceEntity, ref buffObj, targetEntity, ref dmgInfo);
                    }
                }

                // 执行受害者的 Buff OnBeHit
                QList<BuffObj>? targetBuffs = null;
                if (f.TryGet<BuffComp>(targetEntity, out var targetBuffComp))
                {
                    targetBuffs = f.ResolveList(targetBuffComp.Buffs);
                    for (int j = 0; j < targetBuffs.Value.Count; j++)
                    {
                        var buffObj = targetBuffs.Value[j];
                        Helper_Buff.OnBeHit(f, targetEntity, ref buffObj, sourceEntity, ref dmgInfo);
                    }
                }

                // 判断受害者是否会被杀死
                if (Helper_Damage.CanBeKilled(f, targetEntity, dmgInfo))
                {
                    // 先执行攻击者的 Buff OnKill
                    if (sourceBuffs.HasValue)
                    {
                        for (int j = 0; j < sourceBuffs.Value.Count; j++)
                        {
                            var buffObj = sourceBuffs.Value[j];
                            Helper_Buff.OnKill(f, sourceEntity, ref buffObj, targetEntity, ref dmgInfo);
                        }
                    }
                    // 再执行受害者的 Buff OnBeKilled
                    if (targetBuffs.HasValue)
                    {
                        for (int j = 0; j < targetBuffs.Value.Count; j++)
                        {
                            var buffObj = targetBuffs.Value[j];
                            Helper_Buff.OnBeKilled(f, targetEntity, ref buffObj, sourceEntity, ref dmgInfo);
                        }
                    }
                }

                // 最后根据结果处理：如果是治疗或者角色非无敌，才会对血量进行调整
                bool isHeal = Helper_Damage.IsHeal(dmgInfo);
                int dmgValue = Helper_Damage.Calculate(f, dmgInfo);
                if (isHeal || targetStats.IsImmune == 0)
                {
                    // 是直接伤害，并且没被打死
                    if (Helper_Damage.IsDirectDamage(dmgInfo) && !Helper_Damage.CanBeKilled(f, targetEntity, dmgInfo))
                    {
                        // 播受击动画
                        // Helper_Anim.PlayAnim(f, targetEntity, AnimationKey.Hit, true);
                    }
                    // 扣血
                    Helper_Stats.ReduceHp(f, targetEntity, dmgValue, true);
                    // 如果杀死了目标
                    if (Helper_Stats.IsDead(f, targetEntity))
                    {
                        f.Events.OnKill(sourceEntity, targetEntity);
                    }
                }

                // 执行添加buff流程
                if (f.TryResolveList(dmgInfo.addBuffs, out var addBuffs))
                {
                    for (int j = 0; j < addBuffs.Count; j++)
                    {
                        var addBuffInfo = addBuffs[j];
                        Helper_Buff.AddBuff(f, addBuffInfo.target, addBuffInfo);
                    }
                }
            }

            // 清理伤害信息
            for (int i = 0; i < dmgInfos.Count; i++)
            {
                Helper_Damage.DestroyDamageInfo(f, dmgInfos[i]);
            }
            dmgInfos.Clear();
        }
    }

}