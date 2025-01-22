using System;
using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class RebornSystem : SystemMainThreadFilter<RebornSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public DeadComp* DeadComp;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            filter.DeadComp->RebornFrame -= 1;
            if (filter.DeadComp->RebornFrame <= 0)
            {
                Reborn(f, filter.Entity);
                f.Remove<DeadComp>(filter.Entity);
            }
        }

        private void Reborn(Frame f, EntityRef entity)
        {
            Helper_Stats.ResetStats(f, entity);
            f.Events.OnReborn(entity);

            var transform = f.Unsafe.GetPointer<Transform2D>(entity);
            transform->Position = Helper_Math.RandomPosition(f, 18);
        }
    }

}