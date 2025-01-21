using System;
using Photon.Deterministic;
using Quantum.Graph.Skill;
using Quantum.Physics2D;
using UnityEngine;

namespace Quantum.Helper
{

    public unsafe class AoeCmd_test_aoe_1 : AoeCmd
    {
        public override bool CheckInArea(Frame frame, EntityRef aoeEntity, AoeComp* aoeComp, Transform2D* aoeTrans, Hit hitInfo)
        {
            FPVector2 center = aoeTrans->Position;
            FPVector2 point = hitInfo.Point;
            FP radius = aoeComp->Radius;
            FP angle = aoeTrans->Rotation;
            FP arcRad = aoeComp->Model.instance.test_aoe_1->arc * FP.Deg2Rad;
            FP startAngle = angle - arcRad / 2;
            FP endAngle = angle + arcRad / 2;
            bool isIn = Helper_Math.IsPointInSector(point, center, radius, startAngle, endAngle);
            return isIn;
        }

        public override void OnCreate(Frame f, EntityRef aoeEntity, AoeComp* aoeComp)
        {
            Transform2D* transAoe = f.Unsafe.GetPointer<Transform2D>(aoeEntity);
            // Helper_Damage.DoDamage(f, new DamageInfo()
            // {
            //     source = aoeComp->Caster,
            //     target = target,
            //     damageType = EDamageInfoType.DirectDamage,
            //     damage = new Damage() { bullet = 7, },
            //     hitRate = 1,
            //     criticalRate = 0,
            //     angle = trans.Rotation,
            // });
            // f.Events.OnHit(target);
            Helper_Move.ReqRotateTo(f, aoeComp->Caster, transAoe->Rotation - FP.Pi / 2);
            var areaEntity = f.ResolveList(aoeComp->entityInArea);
            for (int i = 0; i < areaEntity.Count; i++)
            {
                var target = areaEntity[i];
                if (f.Exists(target.entity) && target.entity != aoeComp->Caster)
                {
                    Helper_Damage.DoDamage(f, new DamageInfo()
                    {
                        source = aoeComp->Caster,
                        target = target.entity,
                        damageType = EDamageInfoType.DirectDamage,
                        damage = new Damage() { aoe = 7, },
                        hitRate = 1,
                        criticalRate = 0,
                    });
                    Log.Debug($"Aoe hit");
                    f.Events.OnHit(target.entity);
                }
            }
        }

        // public override void OnEntityEnter(Frame f, EntityRef entity, AoeComp* aoeComp, ReadOnlySpan<EntityRef> targets, int count)
        // {
        // }
    }

}