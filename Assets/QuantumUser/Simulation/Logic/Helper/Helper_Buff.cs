using System.Collections.Generic;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public struct DamageInfo
    {
    }

    public static unsafe partial class Helper_Buff
    {
        // Buff id -> BuffCmd
        private static Dictionary<int, BuffCmd> _buffCmds;

        public static void OnAdd(Frame f, EntityRef entity, ref BuffObj buffObj, int modifyStack)
        {
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffCmd))
            {
                buffCmd.OnAdd(f, entity, ref buffObj, modifyStack);
            }
        }

        public static void OnRemove(Frame f, EntityRef entity, ref BuffObj buffObj)
        {
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffCmd))
            {
                buffCmd.OnRemove(f, entity, ref buffObj);
            }
        }

        public static void OnTick(Frame f, EntityRef entity, ref BuffObj buffObj)
        {
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffCmd))
            {
                buffCmd.OnTick(f, entity, ref buffObj);
            }
        }

        public static void OnCaskSkill(Frame f,
                                       EntityRef entity,
                                       ref BuffObj buffObj,
                                       SkillObj skillObj,
                                       ref TimelineObj timelineObj)
        {
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffCmd))
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
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffCmd))
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
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffCmd))
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
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffCmd))
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
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffCmd))
            {
                buffCmd.OnBeKilled(f, entity, ref buffObj, attacker, ref damageInfo);
            }
        }

        public static void AddBuff(Frame f, EntityRef target, AddBuffInfo addBuffInfo)
        {
        }
    }

}