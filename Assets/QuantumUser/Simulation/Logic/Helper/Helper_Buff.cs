using System.Collections.Generic;
using Quantum.Collections;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public static unsafe partial class Helper_Buff
    {
        public static void AddBuff(Frame f, EntityRef target, in AddBuffInfo addBuffInfo)
        {
            if (f.Unsafe.TryGetPointer<BuffComp>(target, out var buffComp))
            {
                var addBuffs = f.ResolveList(buffComp->AddBuffs);
                addBuffs.Add(addBuffInfo);
            }
        }

        public static void ReduceBuffStack(Frame f, EntityRef target, int buffType, EntityRef caster, int reduceCount)
        {
            if (f.Unsafe.TryGetPointer<BuffComp>(target, out var buffComp))
            {
                var buffs = f.ResolveList(buffComp->Buffs);
                if (TryGetBuff(buffs, buffType, caster, out int buffIndex))
                {
                    var buff = buffs[buffIndex];
                    buff.stack -= reduceCount;
                    buffs[buffIndex] = buff;
                    // 标记重新计算属性
                    buffComp->isDirty = true;
                }
            }
        }

        public static bool TryGetBuff(in QList<BuffObj> buffs, int buffType, EntityRef caster, out int buffIndex)
        {
            buffIndex = -1;
            for (int i = 0; i < buffs.Count; i++)
            {
                var buff = buffs[i];
                if (buff.model.type == buffType && (caster == EntityRef.None || buff.caster == caster))
                {
                    buffIndex = i;
                    return true;
                }
            }
            return false;
        }

        #region Buff Cmds

        // Buff id -> BuffCmd
        private static Dictionary<int, BuffCmd> _buffCmds;

        public static void OnAdd(Frame f, EntityRef entity, ref BuffObj buffObj, int modifyStack)
        {
            if (_buffCmds.TryGetValue(buffObj.model.type, out var buffCmd))
            {
                buffCmd.OnAdd(f, entity, ref buffObj, modifyStack);
            }
        }

        public static void OnRemove(Frame f, EntityRef entity, ref BuffObj buffObj)
        {
            if (_buffCmds.TryGetValue(buffObj.model.type, out var buffCmd))
            {
                buffCmd.OnRemove(f, entity, ref buffObj);
            }
        }

        public static void OnTick(Frame f, EntityRef entity, ref BuffObj buffObj)
        {
            if (_buffCmds.TryGetValue(buffObj.model.type, out var buffCmd))
            {
                buffCmd.OnTick(f, entity, ref buffObj);
            }
        }

        public static void OnCastSkill(Frame f,
                                       EntityRef entity,
                                       ref BuffObj buffObj,
                                       SkillObj skillObj,
                                       ref TimelineObj timelineObj)
        {
            if (_buffCmds.TryGetValue(buffObj.model.type, out var buffCmd))
            {
                buffCmd.OnCastSkill(f, entity, ref buffObj, skillObj, ref timelineObj);
            }
        }

        public static void OnHit(Frame f,
                                 EntityRef entity,
                                 ref BuffObj buffObj,
                                 EntityRef targetEntity,
                                 ref DamageInfo damageInfo)
        {
            if (_buffCmds.TryGetValue(buffObj.model.type, out var buffCmd))
            {
                buffCmd.OnHit(f, entity, ref buffObj, targetEntity, ref damageInfo);
            }
        }

        public static void OnBeHit(Frame f,
                                   EntityRef entity,
                                   ref BuffObj buffObj,
                                   EntityRef attacker,
                                   ref DamageInfo damageInfo)
        {
            if (_buffCmds.TryGetValue(buffObj.model.type, out var buffCmd))
            {
                buffCmd.OnBeHit(f, entity, ref buffObj, attacker, ref damageInfo);
            }
        }

        public static void OnKill(Frame f,
                                  EntityRef entity,
                                  ref BuffObj buffObj,
                                  EntityRef target,
                                  ref DamageInfo damageInfo)
        {
            if (_buffCmds.TryGetValue(buffObj.model.type, out var buffCmd))
            {
                buffCmd.OnKill(f, entity, ref buffObj, target, ref damageInfo);
            }
        }

        public static void OnBeKilled(Frame f,
                                      EntityRef entity,
                                      ref BuffObj buffObj,
                                      EntityRef attacker,
                                      ref DamageInfo damageInfo)
        {
            if (_buffCmds.TryGetValue(buffObj.model.type, out var buffCmd))
            {
                buffCmd.OnBeKilled(f, entity, ref buffObj, attacker, ref damageInfo);
            }
        }

        #endregion

        public class BuffCompare : IComparer<BuffObj>
        {
            public static BuffCompare Instance = new BuffCompare();

            public int Compare(BuffObj a, BuffObj b)
            {
                return a.model.priority.CompareTo(b.model.priority);
            }
        }
    }

}