using Quantum.Cfg_;
using Quantum.Helper;
using UnityEngine;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerJoinSystem : SystemSignalsOnly, ISignalOnPlayerAdded, ISignalOnPlayerRemoved
    {
        public override void OnInit(Frame f)
        {
            Cfg.Init();
        }

        public void OnPlayerAdded(Frame f, PlayerRef playerRef, bool firstTime)
        {
            if (firstTime)
            {
                RuntimePlayer runtimePlayer = f.GetPlayerData(playerRef);
                Log.Debug($"Player join {runtimePlayer.heroData.name}");
                var playerEntity = f.Create(runtimePlayer.PlayerAvatar);
                f.AddOrGet<PlayerComp>(playerEntity, out var playerComp);
                playerComp->PlayerRef = playerRef;
                f.Signals.OnPlayerSpawned(playerEntity, playerRef);

                var transform = f.Unsafe.GetPointer<Transform2D>(playerEntity);
                transform->Position = Helper_Math.RandomPosition(f, 18);
            }
        }

        public void OnPlayerRemoved(Frame f, PlayerRef player)
        {
        }
    }

}