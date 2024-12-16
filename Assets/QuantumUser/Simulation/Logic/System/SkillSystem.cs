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
            }
        }

        public void OnPlayerSpawned(Frame f, EntityRef entityRef)
        {
            Log.Debug($"SkillSystem OnPlayerSpawned");
            if (!Helper_Skill.LearnSkill(f, entityRef, SkillId.Knight))
            {
                Log.Error($"Learn skill failed {SkillId.Knight}");
            }
            if (!Helper_Skill.LearnSkill(f, entityRef, SkillId.KnightAttack))
            {
                Log.Error($"Learn skill failed {SkillId.Knight}");
            }
            // if (!Helper_Skill.LearnSkill(f, entityRef, SkillId.Fireball))
            // {
            //     Log.Error($"Learn skill failed {SkillId.Fireball}");
            // }
        }
    }

}