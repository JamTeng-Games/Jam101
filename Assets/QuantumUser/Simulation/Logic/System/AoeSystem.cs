using System;
using System.Runtime.InteropServices;
using Quantum.Collections;
using Quantum.Graph.Skill;
using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class AoeSystem : SystemMainThreadFilter<AoeSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public AoeComp* AoeComp;
            public Transform2D* Transform;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var aoeComp = filter.AoeComp;
            var aoeEntity = filter.Entity;

            // Aoe位移 (下一帧数起效)
            (FPVector2 moveOffset, FP angle) = Helper_Aoe.OnTween(f, aoeEntity, aoeComp, default);
            Log.Debug($"AoeSystem OnTween moveOffset:{moveOffset} angle:{angle}");
            Helper_Move.ReqRotateTo(f, aoeEntity, angle);
            Helper_Move.ReqMove(f, aoeEntity, default, moveOffset);

            // 刚创建的
            if (aoeComp->ElapsedFrame == 0)
            {
                CatchBullet(f, aoeEntity, aoeComp, filter.Transform, false);
                CatchEntity(f, aoeEntity, aoeComp, filter.Transform, false);
                Helper_Aoe.OnCreate(f, aoeEntity, aoeComp);
            }
            // 不是刚创建的
            else
            {
                CatchBullet(f, aoeEntity, aoeComp, filter.Transform, true);
                CatchEntity(f, aoeEntity, aoeComp, filter.Transform, true);
            }

            // Aoe tick
            aoeComp->ElapsedFrame++;
            aoeComp->RemainFrame--;
            if (aoeComp->ElapsedFrame % aoeComp->TickTime == 0)
                Helper_Aoe.OnTick(f, aoeEntity, aoeComp);

            // 生命周期
            if (aoeComp->RemainFrame <= 0)
            {
                Helper_Aoe.OnRemove(f, aoeEntity, aoeComp);
                f.Destroy(aoeEntity);
            }
        }

        // 捕获子弹
        private void CatchBullet(Frame f, EntityRef aoeEntity, AoeComp* aoeComp, Transform2D* aoeTransform, bool sendEvent)
        {
            var layer = f.Layers.GetLayerMask("Bullet");

            Shape2D shape = Shape2D.CreateCircle(aoeComp->Model.radius);
            var hits = f.Physics2D.OverlapShape(aoeTransform->Position, FP._0, shape, layer, options: QueryOptions.HitAll);
            if (hits.Count > 0)
                Log.Info($"Hit bullet {hits.Count}");

            var inAreaBullets = f.ResolveList(aoeComp->bulletInArea);
            Span<AoeEntityRecord> toRemove = stackalloc AoeEntityRecord[inAreaBullets.Count];
            int toRemoveCount = 0;
            // Remove
            for (int i = inAreaBullets.Count - 1; i >= 0; i--)
            {
                var bulletEntity = inAreaBullets[i].entity;
                bool find = false;
                for (int j = 0; j < hits.Count; j++)
                {
                    if (hits[j].Entity == bulletEntity)
                    {
                        if (Helper_Aoe.CheckInArea(f, aoeEntity, aoeComp, aoeTransform, hits[j]))
                            find = true;
                        break;
                    }
                }
                if (!find)
                {
                    toRemove[toRemoveCount++] = inAreaBullets[i];
                    // TODO: RemoveSwap
                    inAreaBullets.RemoveAt(i);
                }
            }
            if (sendEvent && toRemoveCount > 0)
                Helper_Aoe.OnBulletExit(f, aoeEntity, aoeComp, toRemove, toRemoveCount);

            // Add
            Span<EntityRef> toAdd = stackalloc EntityRef[hits.Count];
            int toAddCount = 0;
            for (int i = 0; i < hits.Count; i++)
            {
                var hit = hits[i];
                var targetEntity = hit.Entity;
                if (IsAlreadyIn(inAreaBullets, targetEntity))
                    continue;

                if (!Helper_Aoe.CheckInArea(f, aoeEntity, aoeComp, aoeTransform, hit))
                    continue;

                toAdd[toAddCount++] = targetEntity;
                inAreaBullets.Add(new AoeEntityRecord() { entity = targetEntity, tickTime = 0, });
            }
            if (sendEvent && toAddCount > 0)
                Helper_Aoe.OnBulletEnter(f, aoeEntity, aoeComp, toAdd, toAddCount);
        }

        // 捕获角色
        private void CatchEntity(Frame f, EntityRef aoeEntity, AoeComp* aoeComp, Transform2D* aoeTransform, bool sendEvent)
        {
            var layer = f.Layers.GetLayerMask("Entity");

            Shape2D shape = Shape2D.CreateCircle(aoeComp->Model.radius);
            var hits = f.Physics2D.OverlapShape(aoeTransform->Position, FP._0, shape, layer, options: QueryOptions.HitAll);

            var inAreaEntities = f.ResolveList(aoeComp->entityInArea);
            Span<AoeEntityRecord> toRemove = stackalloc AoeEntityRecord[inAreaEntities.Count];
            int toRemoveCount = 0;
            // Remove
            for (int i = inAreaEntities.Count - 1; i >= 0; i--)
            {
                var entity = inAreaEntities[i].entity;
                bool find = false;
                for (int j = 0; j < hits.Count; j++)
                {
                    if (hits[j].Entity == entity)
                    {
                        if (Helper_Aoe.CheckInArea(f, aoeEntity, aoeComp, aoeTransform, hits[j]))
                            find = true;
                        break;
                    }
                }
                if (!find)
                {
                    toRemove[toRemoveCount++] = inAreaEntities[i];
                    // TODO: RemoveSwap
                    inAreaEntities.RemoveAt(i);
                }
            }
            if (sendEvent && toRemoveCount > 0)
                Helper_Aoe.OnEntityExit(f, aoeEntity, aoeComp, toRemove, toRemoveCount);

            // Add
            Span<EntityRef> toAdd = stackalloc EntityRef[hits.Count];
            int toAddCount = 0;
            for (int i = 0; i < hits.Count; i++)
            {
                var hit = hits[i];
                var targetEntity = hit.Entity;
                if (IsAlreadyIn(inAreaEntities, targetEntity))
                    continue;

                if (!Helper_Aoe.CheckInArea(f, aoeEntity, aoeComp, aoeTransform, hit))
                    continue;

                toAdd[toAddCount++] = targetEntity;
                inAreaEntities.Add(new AoeEntityRecord() { entity = targetEntity, tickTime = 0, });
            }
            if (sendEvent && toAddCount > 0)
                Helper_Aoe.OnEntityEnter(f, aoeEntity, aoeComp, toAdd, toAddCount);
        }

        private bool IsAlreadyIn(in QList<AoeEntityRecord> list, EntityRef entity)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].entity == entity)
                {
                    return true;
                }
            }
            return false;
        }
    }

}