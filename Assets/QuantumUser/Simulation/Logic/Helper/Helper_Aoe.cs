using System;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Physics2D;

namespace Quantum.Helper
{

    public static unsafe partial class Helper_Aoe
    {
        public static void Create(Frame f, CreateAoeInfo createAoeInfo)
        {
            if (f.Unsafe.TryGetPointerSingleton<SCreateAoeInfoComp>(out var createAoeComp))
            {
                // TODO: 在这里选择目标，调整Aoe的角度
                var infos = f.ResolveList(createAoeComp->CreateAoeInfos);
                infos.Add(createAoeInfo);
            }
        }

        #region Aoe Cmds

        // Aoe id -> AoeCmd
        private static Dictionary<int, AoeCmd> _aoeCmds;

        public static void OnCreate(Frame f, EntityRef entity, AoeComp* aoeComp)
        {
            if (_aoeCmds.TryGetValue(aoeComp->Model.type, out var aoeCmd))
            {
                aoeCmd.OnCreate(f, entity, aoeComp);
            }
        }

        public static void OnRemove(Frame f, EntityRef entity, AoeComp* aoeComp)
        {
            if (_aoeCmds.TryGetValue(aoeComp->Model.type, out var aoeCmd))
            {
                aoeCmd.OnRemove(f, entity, aoeComp);
            }
        }

        public static void OnTick(Frame f, EntityRef entity, AoeComp* aoeComp)
        {
            if (_aoeCmds.TryGetValue(aoeComp->Model.type, out var aoeCmd))
            {
                aoeCmd.OnTick(f, entity, aoeComp);
            }
        }

        public static (FPVector2, FP) OnTween(Frame f, EntityRef entity, AoeComp* aoeComp, EntityRef target)
        {
            if (_aoeCmds.TryGetValue(aoeComp->Model.type, out var aoeCmd))
            {
                return aoeCmd.OnTween(f, entity, aoeComp, target);
            }
            return default;
        }

        public static bool CheckInArea(Frame f, EntityRef entity, AoeComp* aoeComp, Transform2D* aoeTrans, Hit hitInfo)
        {
            if (_aoeCmds.TryGetValue(aoeComp->Model.type, out var aoeCmd))
            {
                return aoeCmd.CheckInArea(f, entity, aoeComp, aoeTrans, hitInfo);
            }
            return false;
        }

        public static void OnEntityEnter(Frame f, EntityRef entity, AoeComp* aoeComp, ReadOnlySpan<EntityRef> targets, int count)
        {
            if (_aoeCmds.TryGetValue(aoeComp->Model.type, out var aoeCmd))
            {
                aoeCmd.OnEntityEnter(f, entity, aoeComp, targets, count);
            }
        }

        public static void OnEntityExit(Frame f, EntityRef entity, AoeComp* aoeComp, ReadOnlySpan<AoeEntityRecord> targets, int count)
        {
            if (_aoeCmds.TryGetValue(aoeComp->Model.type, out var aoeCmd))
            {
                aoeCmd.OnEntityExit(f, entity, aoeComp, targets, count);
            }
        }

        public static void OnBulletEnter(Frame f, EntityRef entity, AoeComp* aoeComp, ReadOnlySpan<EntityRef> targets, int count)
        {
            if (_aoeCmds.TryGetValue(aoeComp->Model.type, out var aoeCmd))
            {
                aoeCmd.OnBulletEnter(f, entity, aoeComp, targets, count);
            }
        }

        public static void OnBulletExit(Frame f, EntityRef entity, AoeComp* aoeComp, ReadOnlySpan<AoeEntityRecord> targets, int count)
        {
            if (_aoeCmds.TryGetValue(aoeComp->Model.type, out var aoeCmd))
            {
                aoeCmd.OnBulletExit(f, entity, aoeComp, targets, count);
            }
        }

        #endregion
    }

}