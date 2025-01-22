using System.Collections.Generic;
using Jam.Core;
using Jam.Runtime.Event;
using Jam.Runtime.Scene_;
using Cysharp.Threading.Tasks;
using Jam.Cfg;
using AssetObjectGraphModel = Quantum.AssetObjectGraphModel;
using RuntimeConfig = Quantum.RuntimeConfig;
using RuntimePlayer = Quantum.RuntimePlayer;

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
            G.Event.Unsubscribe(GlobalEventId.ExitCombat, OnExitCombat);
        }

        public override void OnTick(float dt)
        {
        }

        private async void StartQuantum()
        {
            G.UI.Open(UIPanelId.Matching);
            var heroData = new RuntimePlayer.HeroData()
            {
                rid = G.Data.UserData.rid,
                name = G.Data.UserData.name,
                hero = G.Data.UserData.hero,
                items = G.Data.UserData.itemBag.ToQuantumBagItems(),
            };

            // var skillGraphs = new List<Quantum.AssetRef<AssetObjectGraphModel>>();
            // List<Quantum.AssetRef<AssetObjectGraphModel>> skillGraphs = null; //new List<Quantum.AssetRef<AssetObjectGraphModel>>();

            string roomId = G.Data.RoomData.id == 0 ? null : G.Data.RoomData.id.ToString();
            await G.Instance.QuantumChannel.ConnectAsync(runtimePlayerData: new RuntimePlayer() { heroData = heroData },
                                                         // runtimeConfig: new RuntimeConfig() { SkillGraphs = skillGraphs },
                                                         roomId: roomId);
            G.UI.CloseAll();
            G.UI.Open(UIPanelId.ArenaMain);
        }

        private async void OnExitCombat()
        {
            JLog.Info("Before Change Home");
            G.UI.CloseAll();
            await SceneMgr.LoadSceneAsync("Home");
            ChangeState<Home>();
            JLog.Info("After Change Home");
        }
    }

}