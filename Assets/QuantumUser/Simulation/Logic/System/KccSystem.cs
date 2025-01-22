using Quantum.Graph.Skill;
using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class KccSystem : SystemMainThreadFilter<KccSystem.Filter>, ISignalOnComponentAdded<KccComp>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public KccComp* KccComp;
            public InputComp* InputComp;
        }

        public override ComponentSet Without => ComponentSet.Create<DeadComp>();

        public void OnAdded(Frame f, EntityRef entity, KccComp* component)
        {
            KccSettings kccSettings = f.FindAsset<KccSettings>(component->Settings.Id);
            component->Acceleration = kccSettings.Acceleration;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            if (Helper_Attrib.TryGetAttribValue(f, filter.Entity, AttributeType.Speed, out var maxSpeed))
                filter.KccComp->MaxSpeed = maxSpeed / (FP._3 * 100);
            KccSettings kccSettings = f.FindAsset<KccSettings>(filter.KccComp->Settings.Id);
            var moveDirection = filter.InputComp->Input.MoveDirection;
            KccMovementData kccMovementData = Helper_KCC.ComputeRawMovement(f, filter.Entity, moveDirection, kccSettings);
            Helper_KCC.SteerAndMove(f, filter.Entity, kccMovementData, kccSettings);
        }
    }

}