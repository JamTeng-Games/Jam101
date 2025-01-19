using System;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Graph.Skill;
using Quantum.Physics2D;

namespace Quantum.Helper
{

    // 执行Buff Command
    public unsafe class AoeCmd
    {
        public virtual void OnCreate(Frame f, EntityRef entity, AoeComp* aoeComp)
        {
        }

        public virtual void OnRemove(Frame f, EntityRef entity, AoeComp* aoeComp)
        {
        }

        public virtual void OnTick(Frame f, EntityRef entity, AoeComp* aoeComp)
        {
            // increase bullet tick
            var bullets = f.ResolveList(aoeComp->bulletInArea);
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                var bulletRec = bullets[i];
                if (!f.Exists(bulletRec.entity))
                    bullets.RemoveAt(i);
                bulletRec.tickTime++;
                bullets[i] = bulletRec;
            }

            // increase entity tick
            var entities = f.ResolveList(aoeComp->entityInArea);
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                var entityRec = entities[i];
                if (!f.Exists(entityRec.entity))
                    entities.RemoveAt(i);
                entityRec.tickTime++;
                entities[i] = entityRec;
            }
        }

        public virtual (FPVector2, FP) OnTween(Frame f, EntityRef entity, AoeComp* aoeComp, EntityRef target)
        {
            TweenType tweenType = (TweenType)aoeComp->Model.tweenType;
            Transform2D trans = f.Get<Transform2D>(entity);
            FP speed = aoeComp->Speed;
            FPVector2 offset = trans.Forward * speed;
            FP angle = trans.Rotation;
            return (offset, angle);
        }

        public bool CheckInArea(Frame frame, EntityRef entity, AoeComp* aoeComp, Transform2D* aoeTrans, Hit hitInfo)
        {
            return true;
        }

        public virtual void OnEntityEnter(Frame f, EntityRef entity, AoeComp* aoeComp, ReadOnlySpan<EntityRef> targets, int count)
        {
            Log.Info($"Entity a {targets[0]}");
        }

        public virtual void OnEntityExit(Frame f, EntityRef entity, AoeComp* aoeComp, ReadOnlySpan<AoeEntityRecord> targets, int count)
        {
            Log.Info($"Entity Exit {targets[0].entity}");
        }

        public virtual void OnBulletEnter(Frame f, EntityRef entity, AoeComp* aoeComp, ReadOnlySpan<EntityRef> targets, int counts)
        {
            Log.Info($"Bullet Enter {targets[0]}");
        }

        public virtual void OnBulletExit(Frame f, EntityRef entity, AoeComp* aoeComp, ReadOnlySpan<AoeEntityRecord> targets, int count)
        {
            Log.Info($"Bullet Exit {targets[0].entity}");
        }
    }

}