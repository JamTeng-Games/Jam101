using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.Login)]
    public partial class LoginPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.Login;

        [SerializeField] private Button _btn_close;
        [SerializeField] private TMP_InputField _input_name;
        [SerializeField] private TMP_InputField _input_pwd;
        [SerializeField] private Button _btn_login;
        [SerializeField] private Button _btn_register;

        private void OnValidate()
        {
            _btn_close = transform.Find("Popup/btn_close").GetComponent<Button>();
            _input_name = transform.Find("Popup/input_name").GetComponent<TMP_InputField>();
            _input_pwd = transform.Find("Popup/input_pwd").GetComponent<TMP_InputField>();
            _btn_login = transform.Find("Popup/btn_login").GetComponent<Button>();
            _btn_register = transform.Find("Popup/btn_register").GetComponent<Button>();
        }
    }

}