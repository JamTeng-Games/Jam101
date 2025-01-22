using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class MoveSystem : SystemMainThreadFilter<MoveSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public MoveComp* MoveComp;
        }

        public override ComponentSet Without => ComponentSet.Create<DeadComp>();

        public override void Update(Frame f, ref Filter filter)
        {
            Helper_Move.Move(f, filter.Entity, filter.MoveComp->Velocity, filter.MoveComp->Offset);
            filter.MoveComp->Velocity = FPVector2.Zero;
            filter.MoveComp->Offset = FPVector2.Zero;
            //     filter.MoveComp->Vector = FPVector2.Zero;
            // else
            // {
            //     f.Events.OnStopMove(filter.Entity);
            // }
        }
    }

}