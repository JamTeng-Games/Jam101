using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class MoveSystem : SystemMainThreadFilter<MoveSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            Helper_Trans.Move(f, filter.Entity, filter.MoveComp->Vector);
            filter.MoveComp->Vector = FPVector2.Zero;
        }

        public struct Filter
        {
            public EntityRef Entity;
            public MoveComp* MoveComp;
        }
    }

}