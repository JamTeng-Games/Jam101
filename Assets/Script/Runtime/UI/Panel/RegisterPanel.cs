using Jam.Core;

namespace Jam.Runtime.UI_
{

    public partial class RegisterPanel
    {
        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_register.onClick.AddListener(OnClickRegister);
        }

        public override void OnClose()
        {
            _btn_register.onClick.RemoveListener(OnClickRegister);
        }

        protected override void OnTick(float dt)
        {
        }

        private void OnClickRegister()
        {
            string username = _input_name.text;
            string pwd = _input_password.text;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pwd))
            {
                // G.UI.ShowTips("请输入账号和密码");
                JLog.Error("请输入账号和密码");
            }
            else
            {
                G.Login.Register(username, pwd);
            }
        }
    }

}