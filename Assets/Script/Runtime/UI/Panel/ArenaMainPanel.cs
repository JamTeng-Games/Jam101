using Jam.Runtime.Event;
using Jam.Runtime.Quantum_;

namespace Jam.Runtime.UI_
{

    public partial class ArenaMainPanel
    {
        private const float _continueKillTimeout = 10;
        private const float _killTipTimeout = 5;

        private int _killCount = 0;
        private bool _isKilling = false;
        private float _killTimer = 0;

        private bool _killTipShowing = false;
        private float _killTipTimer = 0;

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
#if UNITY_EDITOR
            _btn_disconnect.onClick.AddListener(OnClickDisconnect);
#endif
            G.Event.Subscribe<int>(GlobalEventId.CombatTimeUpdate, OnTimeUpdate);
            G.Event.Subscribe<string>(GlobalEventId.KillHero, OnKillHero);
        }

        public override void OnClose()
        {
#if UNITY_EDITOR
            _btn_disconnect.onClick.RemoveListener(OnClickDisconnect);
#endif
            G.Event.Unsubscribe<int>(GlobalEventId.CombatTimeUpdate, OnTimeUpdate);
            G.Event.Unsubscribe<string>(GlobalEventId.KillHero, OnKillHero);
        }

        protected override void OnTick(float dt)
        {
            if (_isKilling)
            {
                _killTimer += dt;
                if (_killTimer >= _continueKillTimeout)
                {
                    _isKilling = false;
                    _killTimer = 0;
                    _killCount = 0;
                    HideKillTip();
                }
            }

            if (_killTipShowing)
            {
                _killTipTimer += dt;
                if (_killTipTimer >= _killTipTimeout)
                {
                    _killTipTimer = 0;
                    HideKillTip();
                }
            }
        }

        private void OnClickDisconnect()
        {
            G.Instance.QuantumChannel.DisconnectAsync(QuantumConnectFailReason.UserRequest);
        }

        private void OnTimeUpdate(int time)
        {
            int minute = time / 60;
            int second = time % 60;
            _txt_time.text = $"{minute:D2}:{second:D2}";
        }

        private void OnKillHero(string obj)
        {
            _isKilling = true;
            _killTimer = 0;
            _killCount++;

            ShowKillTip();
        }

        private void ShowKillTip()
        {
            _node_kill1.gameObject.SetActive(_killCount == 1);
            _node_kill2.gameObject.SetActive(_killCount == 2);
            _node_kill3.gameObject.SetActive(_killCount == 3);
            _node_kill4.gameObject.SetActive(_killCount == 4);
            _node_kill5.gameObject.SetActive(_killCount >= 5);
            _killTipShowing = true;
        }

        private void HideKillTip()
        {
            _node_kill1.gameObject.SetActive(false);
            _node_kill2.gameObject.SetActive(false);
            _node_kill3.gameObject.SetActive(false);
            _node_kill4.gameObject.SetActive(false);
            _node_kill5.gameObject.SetActive(false);
            _killTipShowing = false;
        }
    }

}