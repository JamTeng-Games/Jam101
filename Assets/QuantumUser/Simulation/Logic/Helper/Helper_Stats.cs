using System;
using Photon.Deterministic;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public static unsafe class Helper_Stats
    {
        public static void InitStats(Frame f, EntityRef entity)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                if (statsComp->IsInit)
                    return;

                Helper_Attrib.TryGetAttribValue(f, entity, AttributeType.MaxHp, out int maxHp);
                statsComp->Hp = maxHp;

                // Init
                statsComp->IsInit = true;
            }
        }

        public static void ResetStats(Frame f, EntityRef entity)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                Helper_Attrib.TryGetAttribValue(f, entity, AttributeType.MaxHp, out int maxHp);
                statsComp->Hp = maxHp;
            }
        }

        public static void AddHp(Frame f, EntityRef entity, int hp, bool showText = false)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                Helper_Attrib.TryGetAttribValue(f, entity, AttributeType.MaxHp, out int maxHp);
                int oldHp = statsComp->Hp;
                statsComp->Hp = Math.Clamp(statsComp->Hp + hp, 0, maxHp);
                Log.Debug($"AddHp: {entity} {statsComp->Hp - oldHp}, res: {statsComp->Hp}");
                if (showText)
                    f.Events.OnChangeHp(entity, statsComp->Hp - oldHp);

                if (statsComp->Hp <= 0)
                {
                    f.Add<DeadComp>(entity, out var deadComp);
                    deadComp->RebornFrame = 90;
                    f.Events.OnDie(entity);
                }
            }
        }

        public static void ReduceHp(Frame f, EntityRef entity, int hp, bool showText = false)
        {
            AddHp(f, entity, -hp, showText);
        }

        // Setter
        public static void SetHp(Frame f, EntityRef entity, int hp, bool showText = false)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                Helper_Attrib.TryGetAttribValue(f, entity, AttributeType.MaxHp, out int maxHp);
                int oldHp = statsComp->Hp;
                statsComp->Hp = Math.Clamp(hp, 0, maxHp);
                if (showText)
                    f.Events.OnChangeHp(entity, statsComp->Hp - oldHp);
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

        public static bool IsDead(Frame f, EntityRef entity)
        {
            if (f.TryGet<StatsComp>(entity, out var statsComp))
            {
                return statsComp.Hp <= 0;
            }
            return false;
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

        public static bool CanRotate(Frame f, EntityRef entity)
        {
            if (f.TryGet<StatsComp>(entity, out var statsComp))
            {
                return statsComp.CanRotate <= 0;
            }
            return true;
        }

        public static bool IsImmune(Frame f, EntityRef entity)
        {
            if (f.TryGet<StatsComp>(entity, out var statsComp))
            {
                return statsComp.IsImmune <= 0;
            }
            return false;
        }

        public static void AddRC_DisableMove(Frame f, EntityRef entity)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->CanMove += 1;
            }
        }

        public static void AddRC_DisableRotate(Frame f, EntityRef entity)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->CanRotate += 1;
            }
        }

        public static void AddRC_DisableSkill(Frame f, EntityRef entity)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->CanUseSkill += 1;
            }
        }

        public static void AddRC_Immune(Frame f, EntityRef entity)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->IsImmune += 1;
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

        public static void ReduceRC_DisableRotate(Frame f, EntityRef entity)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->CanRotate -= 1;
                if (statsComp->CanRotate < 0)
                {
                    statsComp->CanRotate = 0;
                    Log.Error("ReduceRC_DisableRotate: CanRotate < 0");
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

        public static void ReduceRC_Immune(Frame f, EntityRef entity)
        {
            if (f.Unsafe.TryGetPointer<StatsComp>(entity, out var statsComp))
            {
                statsComp->IsImmune -= 1;
                if (statsComp->IsImmune < 0)
                {
                    statsComp->IsImmune = 0;
                    Log.Error("ReduceRC_DisableMove: canMove < 0");
                }
            }
        }
    }

}