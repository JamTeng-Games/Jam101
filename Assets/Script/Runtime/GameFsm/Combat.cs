using cfg;
using Jam.Core;
using Jam.Runtime.Event;
using Jam.Runtime.Scene_;
using Cysharp.Threading.Tasks;

namespace Jam.Runtime.GameFsm
{

    public class Combat : Fsm.State
    {
        public override void OnEnter(Fsm.State fromState)
        {
            JLog.Info("Enter Combat");
            // Change scene
            StartQuantum();
            G.Event.Subscribe(GlobalEventId.ExitCombat, OnExitCombat);
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
        }

        private async void StartQuantum()
        {
            G.UI.Open(UIPanelId.Matching);
            await G.Instance.QuantumChannel.ConnectAsync();
            G.UI.CloseAll();
            G.UI.Open(UIPanelId.ArenaMain);
        }

        private async void OnExitCombat()
        {
            await SceneMgr.LoadSceneAsync("Home");
            G.UI.CloseAll();
            ChangeState<Home>();
        }
    }

}