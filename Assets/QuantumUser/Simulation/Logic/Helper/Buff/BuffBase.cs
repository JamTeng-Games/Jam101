namespace Quantum.Helper
{

    public class BuffBase
    {
        public virtual void OnAdd(Frame f, EntityRef entity, ref BuffObj buffObj, int modifyStack)
        {
        }

        public virtual void OnRemove(Frame f, EntityRef entity, ref BuffObj buffObj)
        {
        }

        public virtual void OnTick(Frame f, EntityRef entity, ref BuffObj buffObj)
        {
        }

        public virtual void OnCastSkill(Frame f,
                                        EntityRef entity,
                                        ref BuffObj buffObj,
                                        SkillObj sKillObj,
                                        ref TimelineObj tlObj)
        {
        }

        public virtual void OnHit(Frame f,
                                  EntityRef entity,
                                  ref BuffObj buffObj,
                                  EntityRef targetEntity,
                                  ref DamageInfo damageInfo)
        {
        }

        public virtual void OnBeHit(Frame f,
                                    EntityRef entity,
                                    ref BuffObj buffObj,
                                    EntityRef attacker,
                                    ref DamageInfo damageInfo)
        {
        }

        public virtual void OnKill(Frame f,
                                   EntityRef entity,
                                   ref BuffObj buffObj,
                                   EntityRef target,
                                   ref DamageInfo damageInfo)
        {
        }

        public virtual void OnBeKilled(Frame f,
                                       EntityRef entity,
                                       ref BuffObj buffObj,
                                       EntityRef attacker,
                                       ref DamageInfo damageInfo)
        {
        }
    }

}