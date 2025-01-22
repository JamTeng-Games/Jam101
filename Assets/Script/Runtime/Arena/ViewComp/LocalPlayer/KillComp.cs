using Jam.Runtime;
using Jam.Runtime.Event;
using Quantum;

namespace Jam.Arena
{

    public class KillComp : JamEntityViewComp
    {
        public override void OnInitialize()
        {
        }

        public override void OnActivate(Frame frame)
        {
            QuantumEvent.UnsubscribeListener(this);
            QuantumEvent.Subscribe<EventOnKill>(listener: this, handler: OnKillHero);
        }

        private void OnKillHero(EventOnKill callback)
        {
            if (callback.source == EntityRef)
            {
                var f = VerifiedFrame;
                if (f.TryGet<PlayerComp>(callback.target, out var targetPlayer))
                {
                    var targetPlayerData = f.GetPlayerData(targetPlayer.PlayerRef);
                    G.Event.Send(GlobalEventId.KillHero, targetPlayerData.heroData.name);
                }
            }
        }
    }

}