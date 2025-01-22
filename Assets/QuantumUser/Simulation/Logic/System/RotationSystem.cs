using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class RotationSystem : SystemMainThreadFilter<RotationSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
            public RotateComp* RotateComp;
        }

        public override ComponentSet Without => ComponentSet.Create<DeadTag>();

        public override void Update(Frame f, ref Filter filter)
        {
            if (Helper_Stats.CanRotate(f, filter.Entity))
            {
                Helper_Move.Rotate(f, filter.Entity, filter.RotateComp->Rotation);
            }
        }
    }

}