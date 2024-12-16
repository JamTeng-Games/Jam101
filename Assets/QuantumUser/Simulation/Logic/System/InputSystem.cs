using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class InputSystem : SystemMainThreadFilter<InputSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            // Player
            if (f.TryGet<PlayerComp>(filter.Entity, out PlayerComp p))
            {
                Log.Debug($"PlayerRef: {(int)p.PlayerRef}");
                var playerInput = *f.GetPlayerInput(p.PlayerRef);
                filter.InputComp->Input = playerInput;
                Helper_Move.ReqMove(f, filter.Entity, playerInput.MoveDirection);
            }
            // AI
            else
            {
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public InputComp* InputComp;
        }
    }

}