using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.Register)]
    public partial class RegisterPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.Register;

        [SerializeField] private Button _btn_close;
        [SerializeField] private TMP_InputField _input_name;
        [SerializeField] private TMP_InputField _input_password;
        [SerializeField] private Button _btn_register;

        private void OnValidate()
        {
            _btn_close = transform.Find("Popup/btn_close").GetComponent<Button>();
            _input_name = transform.Find("Popup/InputFields/input_name").GetComponent<TMP_InputField>();
            _input_password = transform.Find("Popup/InputFields/input_password").GetComponent<TMP_InputField>();
            _btn_register = transform.Find("Popup/btn_register").GetComponent<Button>();
        }
    }

}