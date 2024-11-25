using System.Collections.Generic;
using Jam.Core;
using Jam.Runtime.Event;

namespace Jam.Runtime.GameFsm
{

    public class Preload : Fsm.State
    {
        private const float Timeout = 10f;
        private Dictionary<string, bool> _loadedFlag;
        private float _stayTime;

        public Preload()
        {
            _loadedFlag = new Dictionary<string, bool>();
        }

        public override void OnEnter(Fsm.State fromState)
        {
            JLog.Info("Enter Preload");
            _stayTime = 0f;
            G.Event.Subscribe(GlobalEventId.LoadCfgSuccess, OnLoadCfgSuccess);
            _loadedFlag.Clear();
            PreloadResources();
        }

        public override void OnExit()
        {
        }

        public override void OnTick(float dt)
        {
            _stayTime += dt;
            if (_stayTime > Timeout)
            {
                //
                JLog.Error("Preload timeout");
                return;
            }

            foreach (var (_, isDone) in _loadedFlag)
            {
                if (!isDone)
                    return;
            }

            ChangeState<Login>();
        }

        private void PreloadResources()
        {
            // Load cfg
            _loadedFlag["cfg"] = false;
            G.Cfg.LoadCfg();
        }

        private void OnLoadCfgSuccess()
        {
            _loadedFlag["cfg"] = true;
        }
    }

}