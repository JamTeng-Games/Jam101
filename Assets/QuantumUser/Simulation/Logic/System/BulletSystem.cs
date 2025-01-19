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
    public unsafe class BulletSystem : SystemMainThreadFilter<BulletSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public BulletComp* BulletComp;
            public Transform2D* Transform;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var bulletComp = filter.BulletComp;
            var bulletEntity = filter.Entity;
            var casterEntity = bulletComp->Caster;

            // 刚创建的
            if (bulletComp->ElapsedFrame == 0)
                Helper_Bullet.OnCreate(f, bulletEntity, bulletComp);

            // 处理子弹命中记录信息
            var hitRecords = f.ResolveList(bulletComp->HitRecords);
            for (int i = hitRecords.Count - 1; i >= 0; i--)
            {
                var hitRecord = hitRecords[i];
                hitRecord.timeToHitAgain--;
                if (hitRecord.timeToHitAgain <= 0 || !f.Exists(hitRecord.target))
                {
                    hitRecords.RemoveAt(i);
                }
            }

            // 子弹位移
            (FPVector2 moveOffset, FP angle) = Helper_Bullet.OnTween(f, bulletEntity, bulletComp, default);
            Helper_Move.ReqRotateTo(f, bulletEntity, angle);
            Helper_Move.ReqMove(f, bulletEntity, default, moveOffset);

            bool needDestroy = false;
            // 碰撞信息
            if (bulletComp->TimeCanHit > 0)
            {
                // 还没到能碰撞的时候
                bulletComp->TimeCanHit--;
            }
            else
            {
                // TODO: 暂时先不考虑阵营
                var layer = f.Layers.GetLayerMask("Static", "Entity");
                Log.Info($"bulletComp->Model.radius: {bulletComp->Model.radius}");
                Shape2D shape = Shape2D.CreateCircle(bulletComp->Model.radius);
                var hits = f.Physics2D.OverlapShape(filter.Transform->Position, FP._0, shape, layer,
                                                    options: QueryOptions.HitAll | QueryOptions.ComputeDetailedInfo);
                int maxHitCount = 3;
                Log.Info($"bulletComp->hits: {hits.Count}");
                int hitCount = Math.Min(maxHitCount, hits.Count);
                for (int i = 0; i < hitCount; i++)
                {
                    var hit = hits[i];
                    var targetEntity = hit.Entity;
                    // 打到了自己 (其实打不到)
                    if (targetEntity == bulletEntity || targetEntity == casterEntity)
                        continue;

                    // 根据HitRecords, 判断能否再次命中
                    if (!Helper_Bullet.CanHit(hitRecords, targetEntity))
                        continue;

                    // target 如果死了就忽略
                    if (Helper_Stats.IsDead(f, targetEntity))
                        continue;

                    // 命中了
                    bulletComp->Hp--;

                    // 命中了墙体
                    if (hit.IsStatic && bulletComp->Model.removeOnObstacle)
                        bulletComp->Hp = 0;

                    Helper_Bullet.OnHit(f, bulletEntity, bulletComp, targetEntity, hit);
                    Log.Debug($"bulletComp->Hp {bulletComp->Hp}");
                    if (bulletComp->Hp > 0)
                    {
                        // 记录Record
                        hitRecords.Add(new BulletHitRecord()
                        {
                            target = targetEntity, timeToHitAgain = bulletComp->Model.sameTargetDelayFrame
                        });
                    }
                    else
                    {
                        Log.Debug($"bulletComp->Model needDestroy");
                        needDestroy = true;
                        break;
                    }
                }
            }

            // 子弹tick
            bulletComp->ElapsedFrame++;
            bulletComp->RemainFrame--;
            Helper_Bullet.OnTick(f, bulletEntity, bulletComp);

            // 生命周期
            if (bulletComp->RemainFrame <= 0 || needDestroy)
            {
                Log.Debug($"bulletComp->Model OnRemove");
                Helper_Bullet.OnRemove(f, bulletEntity, bulletComp);
                f.Destroy(bulletEntity);
            }
        }
    }

}