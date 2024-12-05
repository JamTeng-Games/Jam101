using Quantum.Collections;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public static unsafe class Helper_Skill
    {
        public static bool LearnSkill(Frame f, EntityRef entity, SkillId skillId)
        {
            if (f.TryGetSingleton<SSkillModelContainerComp>(out var skillModelComp))
            {
                var models = f.ResolveDictionary(skillModelComp.Models);
                if (models.TryGetValue((int)skillId, out var skillModel))
                {
                    f.AddOrGet<SkillComp>(entity, out var skillComp);
                    var skillList = f.ResolveList(skillComp->Skills);
                    SkillObj skillObj = new SkillObj() { level = 1, cd = 0, model = skillModel };
                    // 已经学过技能，不能再学
                    if (!IsSkillLearned(skillList, skillId))
                    {
                        skillList.Add(skillObj);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CastAttackSkill(Frame f, EntityRef entity)
        {
            // Find attack skill id
            if (f.Unsafe.TryGetPointer<SkillComp>(entity, out var skillComp))
            {
                var skillList = f.ResolveList(skillComp->Skills);
                int skillIndex = -1;
                for (int i = 0; i < skillList.Count; i++)
                {
                    if (skillList[i].model.isAttack)
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

        public static bool CastSkill(Frame f, EntityRef entity, SkillId skillId)
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

        public static bool TryGetSkillObj(Frame f, EntityRef entity, SkillId skillId, out SkillObj skillObj)
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

        public static bool IsSkillLearned(Frame f, EntityRef entity, SkillId skillId)
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
        private static bool IsSkillLearned(QList<SkillObj> skills, SkillId skillId)
        {
            foreach (var skillObj in skills)
            {
                if (skillObj.model.id == (int)skillId)
                    return true;
            }
            return false;
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
                        Helper_Buff.OnCaskSkill(f, entity, ref buffTmp, skillObj, ref tlObj);
                        buffs[i] = buffTmp;
                    }
                }
                // TODO: Do resource cost
                Helper_Timeline.AddTimelineObj(f, tlObj);
            }

            // 因为是struct，需要赋值回去
            skills[skillIndex] = skillObj;
            return true;
        }
    }

}