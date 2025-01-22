using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class InputSystem : SystemMainThreadFilter<InputSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public InputComp* InputComp;
        }

        public override ComponentSet Without => ComponentSet.Create<DeadTag>();

        public override void Update(Frame f, ref Filter filter)
        {
            // Player
            if (f.TryGet<PlayerComp>(filter.Entity, out PlayerComp p))
            {
                Log.Debug($"PlayerRef: {(int)p.PlayerRef}");

                // // TODO: Only for debug
                // if (p.PlayerRef != 0)
                //     return;

                var playerInput = *f.GetPlayerInput(p.PlayerRef);
                filter.InputComp->Input = playerInput;
            }
            // AI
            else
            {
            }
        }
    }

}