using System;
using Photon.Deterministic;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public static unsafe class Helper_Stats
    {
        // Setter
        public static void SetHp(Frame f, EntityRef entity, int hp)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                Helper_Attrib.TryGetAttribValue(f, entity, AttributeType.MaxHp, out int maxHp);
                statsComp->Hp = Math.Clamp(hp, 0, maxHp);
            }
        }

        public static void SetDefence(Frame f, EntityRef entity, int defence)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->Defence = Math.Max(0, defence);
            }
        }

        public static void SetAttackClip(Frame f, EntityRef entity, int attackClip)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->AttackClip = Math.Max(0, attackClip);
            }
        }

        public static void SetSuperPower(Frame f, EntityRef entity, int superPower)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->SuperPower = Math.Max(0, superPower);
            }
        }

        // Getter
        public static int GetHp(Frame f, EntityRef entity)
        {
            if (f.TryGet<StatsComp>(entity, out var statsComp))
            {
                return statsComp.Hp;
            }
            return -1;
        }

        public static int GetMp(Frame f, EntityRef entity)
        {
            if (f.TryGet<StatsComp>(entity, out var statsComp))
            {
                return statsComp.Mp;
            }
            return -1;
        }

        public static int GetDefence(Frame f, EntityRef entity)
        {
            if (f.TryGet<StatsComp>(entity, out var statsComp))
            {
                return statsComp.Defence;
            }
            return -1;
        }

        public static int GetAttackClip(Frame f, EntityRef entity)
        {
            if (f.TryGet<StatsComp>(entity, out var statsComp))
            {
                return statsComp.AttackClip;
            }
            return -1;
        }

        public static int GetSuperPower(Frame f, EntityRef entity)
        {
            if (f.TryGet<StatsComp>(entity, out var statsComp))
            {
                return statsComp.SuperPower;
            }
            return -1;
        }

        public static bool CanMove(Frame f, EntityRef entity)
        {
            if (f.TryGet<StatsComp>(entity, out var statsComp))
            {
                return statsComp.CanMove <= 0;
            }
            return true;
        }

        public static bool CanUseSkill(Frame f, EntityRef entity)
        {
            if (f.TryGet<StatsComp>(entity, out var statsComp))
            {
                return statsComp.CanUseSkill <= 0;
            }
            return true;
        }

        public static void AddRC_DisableMove(Frame f, EntityRef entity)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->CanMove += 1;
            }
        }

        public static void AddRC_DisableSkill(Frame f, EntityRef entity)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->CanUseSkill += 1;
            }
        }

        public static void ReduceRC_DisableMove(Frame f, EntityRef entity)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->CanMove -= 1;
                if (statsComp->CanMove < 0)
                {
                    statsComp->CanMove = 0;
                    Log.Error("ReduceRC_DisableMove: canMove < 0");
                }
            }
        }

        public static void ReduceRC_DisableSkill(Frame f, EntityRef entity)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->CanUseSkill -= 1;
                if (statsComp->CanUseSkill < 0)
                {
                    statsComp->CanUseSkill = 0;
                    Log.Error("ReduceRC_DisableMove: canMove < 0");
                }
            }
        }
    }

}