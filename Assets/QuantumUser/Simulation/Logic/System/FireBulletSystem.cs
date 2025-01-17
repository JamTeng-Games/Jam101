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
    public unsafe class FireBulletSystem : SystemMainThread
    {
        public override void OnInit(Frame f)
        {
            f.GetOrAddSingleton<SBulletFireInfoComp>();
        }

        public override void Update(Frame f)
        {
            if (!f.Unsafe.TryGetPointerSingleton<SBulletFireInfoComp>(out var bulletFireInfoComp))
                return;

            var bulletFireInfos = f.ResolveList(bulletFireInfoComp->FireBulletInfos);
            for (int i = 0; i < bulletFireInfos.Count; i++)
            {
                FireBulletInfo bulletFireInfo = bulletFireInfos[i];

                // Create Entity
                EntityRef bulletEntity = f.Create(f.RuntimeConfig.BulletPrototype);

                // Transform
                Transform2D* trans = f.Unsafe.GetPointer<Transform2D>(bulletEntity);
                trans->Position = bulletFireInfo.firePos;
                trans->Rotation = bulletFireInfo.fireAngle + FP.Pi / 2;
                // trans->Rotation = bulletFireInfo.fireAngle;

                // BulletComp
                BulletComp* bulletComp = f.Unsafe.GetPointer<BulletComp>(bulletEntity);
                bulletComp->Model = bulletFireInfo.model;
                bulletComp->Hp = bulletFireInfo.hitTimes;
                bulletComp->ElapsedFrame = 0;
                bulletComp->RemainFrame = bulletFireInfo.duration;
                bulletComp->TimeCanHit = bulletFireInfo.timeCanHit;
                bulletComp->Speed = bulletFireInfo.speed / FP._100 / FP._3;
                bulletComp->Caster = bulletFireInfo.caster;
                // TODO: 保存释放着当前的状态值，目前先根据角色当前属性值来计算

                // TODO: 自动瞄准方向
            }

            bulletFireInfos.Clear();
        }
    }

}