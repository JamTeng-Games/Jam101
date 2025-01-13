using Jam.Cfg;
using Jam.Core;

namespace Jam.Runtime.UI_
{

    public partial class LoginPanel
    {
        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_login.onClick.AddListener(OnClickLogin);
            _btn_register.onClick.AddListener(OnClickRegister);
        }

        public override void OnClose()
        {
            _btn_login.onClick.RemoveListener(OnClickLogin);
            _btn_register.onClick.RemoveListener(OnClickRegister);
        }

        protected override void OnTick(float dt)
        {
        }

        private void OnClickLogin()
        {
            string username = _input_name.text;
            string pwd = _input_pwd.text;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pwd))
            {
                // G.UI.ShowTips("请输入账号和密码");
                JLog.Error("请输入账号和密码");
            }
            else
            {
                G.Login.Login(username, pwd);
            }
        }

        private void OnClickRegister()
        {
            G.UI.Open(UIPanelId.Register, UIShowMode.Replace);
        }
    }

}