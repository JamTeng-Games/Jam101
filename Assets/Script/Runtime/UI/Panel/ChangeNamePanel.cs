using Jam.Runtime.Data_;
using Jam.Runtime.Net_;

namespace Jam.Runtime.UI_
{

    public partial class ChangeNamePanel
    {
        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_ok.onClick.AddListener(OnClickedOk);
            _btn_close.gameObject.SetActive(false);
            // _btn_close.onClick.AddListener(OnClickedClose);
        }

        public override void OnClose()
        {
            _btn_ok.onClick.RemoveListener(OnClickedOk);
            // _btn_close.onClick.RemoveListener(OnClickedClose);
        }

        protected override void OnTick(float dt)
        {
        }

        private void OnClickedOk()
        {
            if (!string.IsNullOrEmpty(_input_name.text))
            {
                G.Net.Send(NetCmd.CS_ChangeName, new ChangeNameReq() { name = _input_name.text });
            }
        }

        private void OnClickedClose()
        {
        }
    }

}