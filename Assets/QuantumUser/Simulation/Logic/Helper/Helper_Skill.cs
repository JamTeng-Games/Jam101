using Quantum.Collections;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public static unsafe class Helper_Skill
    {
        public static bool LearnSkill(Frame f, EntityRef entity, int skillId, int stack = 1)
        {
            if (f.TryGetSingleton<SSkillModelContainerComp>(out var skillModelComp))
            {
                var models = f.ResolveDictionary(skillModelComp.Models);
                if (models.TryGetValue(skillId, out var skillModel))
                {
                    f.AddOrGet<SkillComp>(entity, out var skillComp);
                    var skillList = f.ResolveList(skillComp->Skills);
                    SkillObj skillObj = new SkillObj() { level = 1, cd = 0, model = skillModel, refCount = stack };

                    int skillIndex = FindSkillIndex(skillList, skillId);
                    // 没学过技能
                    if (skillIndex == -1)
                    {
                        skillList.Add(skillObj);

                        // Add buff
                        var addBuffs = f.ResolveList(skillModel.addBuffs);
                        for (int i = 0; i < addBuffs.Count; i++)
                        {
                            var addBuffInfo = addBuffs[i];
                            addBuffInfo.addStack = stack;
                            addBuffInfo.caster = entity;
                            Helper_Buff.AddBuff(f, entity, addBuffInfo);
                        }
                        return true;
                    }
                    // 已经学过技能, 并且这个技能可以学多次
                    if (skillModel.canLearnMulti)
                    {
                        var oldSkillObj = skillList[skillIndex];
                        oldSkillObj.refCount += stack;
                        skillList[skillIndex] = oldSkillObj;
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool ForgetSkill(Frame f, EntityRef entity, int skillId)
        {
            if (f.Unsafe.TryGetPointer<SkillComp>(entity, out var skillComp))
            {
                var skillList = f.ResolveList(skillComp->Skills);
                int skillIndex = FindSkillIndex(skillList, skillId);
                if (skillIndex == -1)
                    return false;

                var skillObj = skillList[skillIndex];
                skillObj.refCount--;
                if (skillObj.refCount == 0)
                    skillList.RemoveAt(skillIndex);
                else
                    skillList[skillIndex] = skillObj;

                // Remove buff
                var addBuffs = f.ResolveList(skillObj.model.addBuffs);
                for (int i = 0; i < addBuffs.Count; i++)
                {
                    var addBuffInfo = addBuffs[i];
                    addBuffInfo.caster = entity;
                    Helper_Buff.ReduceBuffStack(f, entity, addBuffInfo.buffModel.type, entity, 1);
                }
                return true;
            }
            return false;
        }

        public static bool CastAttackSkill(Frame f, EntityRef entity)
        {
            return CastSkillByType(f, entity, SkillType.Attack);
        }

        public static bool CastHeroSkill(Frame f, EntityRef entity)
        {
            return CastSkillByType(f, entity, SkillType.HeroSkill);
        }

        public static bool CastSuperSkill(Frame f, EntityRef entity)
        {
            return CastSkillByType(f, entity, SkillType.SuperSkill);
        }

        public static bool CastSkillByType(Frame f, EntityRef entity, SkillType skillType)
        {
            // Find attack skill id
            if (f.Unsafe.TryGetPointer<SkillComp>(entity, out var skillComp))
            {
                var skillList = f.ResolveList(skillComp->Skills);
                int skillIndex = -1;
                for (int i = 0; i < skillList.Count; i++)
                {
                    if (skillList[i].model.type == (int)skillType)
                    {
                        skillIndex = i;
                        break;
                    }
                }
                if (skillIndex == -1)
                {
                    Log.Error($"No attack skill found for entity {entity}");
                    return false;
                }
                return CastSkillImpl(f, entity, skillComp, skillIndex);
            }
            return false;
        }

        public static bool CastSkill(Frame f, EntityRef entity, int skillId)
        {
            if (f.Unsafe.TryGetPointer<SkillComp>(entity, out var skillComp))
            {
                var skillList = f.ResolveList(skillComp->Skills);
                int skillIndex = -1;
                for (int i = 0; i < skillList.Count; i++)
                {
                    var skillObj = skillList[i];
                    if (skillObj.model.id == (int)skillId)
                    {
                        skillIndex = i;
                        break;
                    }
                }
                if (skillIndex == -1)
                {
                    Log.Error($"Skill {skillId} not found for entity {entity}");
                    return false;
                }
                return CastSkillImpl(f, entity, skillComp, skillIndex);
            }
            return false;
        }

        public static bool TryGetSkillObj(Frame f, EntityRef entity, int skillId, out SkillObj skillObj)
        {
            if (f.Unsafe.TryGetPointer<SkillComp>(entity, out var skillComp))
            {
                var skillList = f.ResolveList(skillComp->Skills);
                for (int i = 0; i < skillList.Count; i++)
                {
                    if (skillList[i].model.id == (int)skillId)
                    {
                        skillObj = skillList[i];
                        return true;
                    }
                }
            }
            skillObj = default;
            return false;
        }

        public static bool IsSkillLearned(Frame f, EntityRef entity, int skillId)
        {
            if (f.TryGet<SkillComp>(entity, out var skillComp))
            {
                var skillList = f.ResolveList(skillComp.Skills);
                for (int i = 0; i < skillList.Count; i++)
                {
                    if (skillList[i].model.id == (int)skillId)
                        return true;
                }
            }
            return false;
        }

        ///////////////////////////// Private methods /////////////////////////////
        private static bool IsSkillLearned(QList<SkillObj> skills, int skillId)
        {
            foreach (var skillObj in skills)
            {
                if (skillObj.model.id == skillId)
                    return true;
            }
            return false;
        }

        private static int FindSkillIndex(QList<SkillObj> skills, int skillId)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].model.id == skillId)
                    return i;
            }
            return -1;
        }

        private static bool CastSkillImpl(Frame f, EntityRef entity, SkillComp* skillComp, int skillIndex)
        {
            var skills = f.ResolveList(skillComp->Skills);
            var skillObj = skills[skillIndex];
            if (skillObj.cd > 0)
                return false;

            // TEMP: 设置cd
            skillObj.cd = skillObj.model.cd;

            // TODO: Check Resource

            // Timeline
            if (Helper_Timeline.TryGetTimelineModel(f, skillObj.model.timelineModelId, out var tlModel))
            {
                TimelineObj tlObj = Helper_Timeline.CreateTimelineObj(tlModel, entity);
                // Do buff OnCastSkill
                if (f.TryGet<BuffComp>(entity, out var buffComp))
                {
                    var buffs = f.ResolveList(buffComp.Buffs);
                    for (int i = 0; i < buffs.Count; i++)
                    {
                        var buffTmp = buffs[i];
                        Helper_Buff.OnCastSkill(f, entity, ref buffTmp, skillObj, ref tlObj);
                        buffs[i] = buffTmp;
                    }
                }
                // TODO: Do resource cost
                Helper_Timeline.AddTimelineObj(f, tlObj, skillObj.model.canInterrupt);
            }

            // 因为是struct，需要赋值回去
            skills[skillIndex] = skillObj;
            return true;
        }
    }

}