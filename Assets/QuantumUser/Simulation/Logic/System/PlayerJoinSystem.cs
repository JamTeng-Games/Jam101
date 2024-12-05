namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerJoinSystem : SystemSignalsOnly, ISignalOnPlayerAdded, ISignalOnPlayerRemoved
    {
        public void OnPlayerAdded(Frame f, PlayerRef playerRef, bool firstTime)
        {
            if (firstTime)
            {
                var runtimePlayer = f.GetPlayerData(playerRef);
                var playerEntity = f.Create(runtimePlayer.PlayerAvatar);
                f.AddOrGet<PlayerComp>(playerEntity, out var playerComp);
                playerComp->PlayerRef = playerRef;
                f.Signals.OnPlayerSpawned(playerEntity);
            }
        }

        public void OnPlayerRemoved(Frame f, PlayerRef player)
        {
        }
    }

}