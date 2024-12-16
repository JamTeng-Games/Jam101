using Jam.Runtime.Quantum_;

namespace Jam.Runtime.UI_
{

    public partial class ArenaMainPanel
    {
        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_disconnect.onClick.AddListener(OnClickDisconnect);
        }

        public override void OnClose()
        {
            _btn_disconnect.onClick.RemoveListener(OnClickDisconnect);
        }

        protected override void OnTick(float dt)
        {
        }

        private void OnClickDisconnect()
        {
            G.Instance.QuantumChannel.DisconnectAsync(QuantumConnectFailReason.UserRequest);
        }
    }

}