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
    public unsafe class CreateAoeSystem : SystemMainThread
    {
        public override void OnInit(Frame f)
        {
            f.GetOrAddSingleton<SCreateAoeInfoComp>();
        }

        public override void Update(Frame f)
        {
            if (!f.Unsafe.TryGetPointerSingleton<SCreateAoeInfoComp>(out var aoeInfoComps))
                return;

            var aoeInfos = f.ResolveList(aoeInfoComps->CreateAoeInfos);
            for (int i = 0; i < aoeInfos.Count; i++)
            {
                CreateAoeInfo aoeInfo = aoeInfos[i];

                // Create Entity
                EntityRef aoeEntity = f.Create(f.RuntimeConfig.AoePrototype);

                // Transform
                Transform2D* trans = f.Unsafe.GetPointer<Transform2D>(aoeEntity);
                trans->Position = aoeInfo.position;
                trans->Rotation = aoeInfo.angle + FP.Pi / 2;

                // AoeComp
                AoeComp* aoeComp = f.Unsafe.GetPointer<AoeComp>(aoeEntity);
                aoeComp->Model = aoeInfo.model;
                aoeComp->ElapsedFrame = 0;
                aoeComp->RemainFrame = aoeInfo.duration;
                aoeComp->Speed = aoeInfo.speed / FP._100 / FP._3;
                aoeComp->TickTime = aoeInfo.model.tickTime;
                aoeComp->Radius = aoeInfo.model.radius;
                aoeComp->Caster = aoeInfo.caster;
                // TODO: 保存释放着当前的状态值，目前先根据角色当前属性值来计算
            }
            aoeInfos.Clear();
        }
    }

}