using Jam.Cfg;
using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;
using Jam.Runtime.Net_;
using UnityEngine;

namespace Jam.Runtime.Login_
{

    public class LoginMgr : IMgr
    {
        private string _ip = "106.14.59.20";
        private int _port = 9901;

        private string _account;
        private string _pwd;
        private int _token;

        public void Init()
        {
            // PlayerPrefs.DeleteAll();
        }

        public void Shutdown(bool isAppQuit)
        {
        }

        public void TryLogin()
        {
            if (!G.Net.IsConnected())
            {
                G.Net.Connect(_ip, _port, succ =>
                {
                    if (succ)
                    {
                        AutoLogin();
                    }
                    else
                    {
                        JLog.Error("Connect to server fail");
                    }
                });
            }
            else
            {
                AutoLogin();
            }
        }

        public void Login(string account, string pwd)
        {
            _pwd = pwd;
            LoginReq req = new LoginReq() { account = account, pwd = pwd };
            JLog.Error("LoginReq: " + req.account + " " + req.pwd);
            G.Net.Send(NetCmd.CS_Login, req);
        }

        private void AutoLogin()
        {
            string account = GetAccount();
            string pwd = GetPwd();
            if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(pwd))
            {
                Login(account, pwd);
            }
            else
            {
                G.UI.Open(UIPanelId.Login);
            }
        }

        public void Register(string account, string pwd)
        {
            _pwd = pwd;
            RegisterReq req = new RegisterReq() { account = account, pwd = pwd };
            G.Net.Send(NetCmd.CS_Register, req);
        }

        public string GetAccount()
        {
            if (!string.IsNullOrEmpty(_account))
                return _account;

            _account = PlayerPrefs.GetString("account", "");
            return _account;
        }

        public string GetPwd()
        {
            if (!string.IsNullOrEmpty(_pwd))
                return _pwd;

            _pwd = PlayerPrefs.GetString("pwd", "");
            return _pwd;
        }

        public void ProcessLoginSuccess(LoginSuccess msg)
        {
            _account = msg.account;
            _token = msg.token;
            PlayerPrefs.SetString("account", _account);
            PlayerPrefs.SetString("pwd", _pwd);
            PlayerPrefs.Save();
            G.Event.Send(GlobalEventId.LoginSuccess);
            G.UI.Close(UIPanelId.Login);
            G.UI.Close(UIPanelId.Register);
        }

        public void ProcessLoginFail(LoginFail msg)
        {
            JLog.Error(msg.code);
            G.UI.Open(UIPanelId.Login);
        }

        public void ProcessRegisterSuccess(RegisterSuccess msg)
        {
            _account = msg.account;
            JLog.Info("Register success: " + _account);
            PlayerPrefs.SetString("account", _account);
            PlayerPrefs.SetString("pwd", _pwd);
            PlayerPrefs.Save();
            AutoLogin();
            G.UI.Close(UIPanelId.Login);
            G.UI.Close(UIPanelId.Register);
        }

        public void ProcessRegisterFail(RegisterFail msg)
        {
            JLog.Error(msg.code);
        }
    }

}