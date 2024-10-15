using J.Core;
using J.Runtime.Home.PlayFab_;

namespace J.Runtime.UI
{
    public partial class LoginPanel : UIPanel
    {
        public override void OnDrop()
        {
        }

        public override void OnLoad()
        {
        }

        public override void OnShow()
        {
            _btn_login.onClick.AddListener(OnLoginBtnClick);
        }

        public override void OnHide()
        {
            _btn_login.onClick.RemoveListener(OnLoginBtnClick);
        }

        public override void OnClose()
        {
        }

        private void OnLoginBtnClick()
        {
            string account = _input_account.text;
            string password = _input_password.text;
            JLog.Info($"account: {account}, password: {password}");
            PlayFabMgr.Login();
        }
    }
}