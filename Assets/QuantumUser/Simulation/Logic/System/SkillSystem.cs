using Quantum.Cfg_;
using Quantum.Graph.Skill;
using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    /// 技能系统, 学习技能，Tick技能，释放技能
    [Preserve]
    public unsafe class SkillSystem : SystemMainThreadFilter<SkillSystem.Filter>, ISignalOnPlayerSpawned
    {
        public struct Filter
        {
            public EntityRef Entity;
            public InputComp* InputComp;
            public SkillComp* SkillComp;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var skills = f.ResolveList(filter.SkillComp->Skills);
            for (int i = 0; i < skills.Count; i++)
            {
                var skillObj = skills[i];
                skillObj.cd -= 1;
                skills[i] = skillObj;
            }

            // 普通攻击
            if (filter.InputComp->Input.Attack.WasPressed)
            {
                Helper_Skill.CastAttackSkill(f, filter.Entity);
                Log.Debug($"Hp {Helper_Stats.GetHp(f, filter.Entity)}");
            }

            // 技能攻击
            if (filter.InputComp->Input.Skill.WasPressed)
            {
                Helper_Skill.CastHeroSkill(f, filter.Entity);
                // Helper_Damage.DoDamage(f, filter.Entity, filter.Entity, EDamageInfoType.DirectDamage,
                //                        new Damage() { bullet = 10, aoe = 0, }, FP._1, FP._0, FP._0_20, 0);
            }
        }

        public void OnPlayerSpawned(Frame f, EntityRef entity, PlayerRef playerRef)
        {
            RuntimePlayer playerData = f.GetPlayerData(playerRef);
            RuntimePlayer.HeroData heroData = playerData.heroData;

            // 英雄技能
            if (Cfg.Tb.TbHero.DataMap.TryGetValue(heroData.hero, out var heroCfg))
            {
                var skillList = heroCfg.SkillList;
                foreach (var skillId in skillList)
                {
                    if (!Helper_Skill.LearnSkill(f, entity, skillId))
                    {
                        Log.Error($"Learn skill failed {skillId}");
                    }
                }
            }

            // 道具添加的技能
            var itemsData = heroData.items;
            if (itemsData == null)
                return;
            var itemTbCfg = Cfg.Tb.TbItem.DataMap;
            foreach (var item in itemsData)
            {
                if (itemTbCfg.TryGetValue(item.id, out var itemCfg))
                {
                    var addSkills = itemCfg.AddSkills;
                    foreach (var skillId in addSkills)
                    {
                        if (skillId > 0 && !Helper_Skill.LearnSkill(f, entity, skillId, item.amount))
                        {
                            Log.Error($"Learn skill failed {skillId}");
                        }
                    }
                }
            }
        }
    }

}