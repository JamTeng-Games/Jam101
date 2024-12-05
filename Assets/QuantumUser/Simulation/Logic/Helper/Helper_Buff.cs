using System.Collections.Generic;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public struct DamageInfo
    {
    }

    public static unsafe class Helper_Buff
    {
        // Buff id -> BuffBase
        private static Dictionary<int, BuffBase> _buffCmds;

        static Helper_Buff()
        {
            _buffCmds = new Dictionary<int, BuffBase>() { };
        }

        public static void OnAdd(Frame f, EntityRef entity, ref BuffObj buffObj, int modifyStack)
        {
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffBase))
            {
                buffBase.OnAdd(f, entity, ref buffObj, modifyStack);
            }
        }

        public static void OnRemove(Frame f, EntityRef entity, ref BuffObj buffObj)
        {
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffBase))
            {
                buffBase.OnRemove(f, entity, ref buffObj);
            }
        }

        public static void OnTick(Frame f, EntityRef entity, ref BuffObj buffObj)
        {
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffBase))
            {
                buffBase.OnTick(f, entity, ref buffObj);
            }
        }

        public static void OnCaskSkill(Frame f,
                                       EntityRef entity,
                                       ref BuffObj buffObj,
                                       SkillObj skillObj,
                                       ref TimelineObj timelineObj)
        {
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffBase))
            {
                buffBase.OnCastSkill(f, entity, ref buffObj, skillObj, ref timelineObj);
            }
        }

        public static void OnHit(Frame f,
                                 EntityRef entity,
                                 ref BuffObj buffObj,
                                 EntityRef targetEntity,
                                 ref DamageInfo damageInfo)
        {
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffBase))
            {
                buffBase.OnHit(f, entity, ref buffObj, targetEntity, ref damageInfo);
            }
        }

        public static void OnBeHit(Frame f,
                                   EntityRef entity,
                                   ref BuffObj buffObj,
                                   EntityRef attacker,
                                   ref DamageInfo damageInfo)
        {
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffBase))
            {
                buffBase.OnBeHit(f, entity, ref buffObj, attacker, ref damageInfo);
            }
        }

        public static void OnKill(Frame f,
                                  EntityRef entity,
                                  ref BuffObj buffObj,
                                  EntityRef target,
                                  ref DamageInfo damageInfo)
        {
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffBase))
            {
                buffBase.OnKill(f, entity, ref buffObj, target, ref damageInfo);
            }
        }

        public static void OnBeKilled(Frame f,
                                      EntityRef entity,
                                      ref BuffObj buffObj,
                                      EntityRef attacker,
                                      ref DamageInfo damageInfo)
        {
            if (_buffCmds.TryGetValue(buffObj.model.id, out var buffBase))
            {
                buffBase.OnBeKilled(f, entity, ref buffObj, attacker, ref damageInfo);
            }
        }
    }

}