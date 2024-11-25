namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class HeroSystem : SystemMainThreadFilter<HeroSystem.Filter>, ISignalOnPlayerAdded
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
            public PhysicsBody2D* Body;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            filter.Transform->Position += new FPVector2(FP._0_01, 0);
        }

        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var runtimePlayer = f.GetPlayerData(player);
            var entity = f.Create(runtimePlayer.PlayerAvatar);

            // var link = new PlayerLink() { Player = player };
            // var targetRot = new TargetRotation() { TargetRot = FPQuaternion.Identity };
            // f.Add(entity, link);
            // f.Add(entity, targetRot);
            //
            // if (f.Unsafe.TryGetPointer<Transform3D>(entity, out var transform))
            // {
            //     transform->Position = new FPVector3(player * 2, 2, 0);
            //     transform->Rotation = FPQuaternion.Identity;
            // }
            //
            // // Event
            // f.Events.PowerRestored(entity, FP._10);
        }
    }

}