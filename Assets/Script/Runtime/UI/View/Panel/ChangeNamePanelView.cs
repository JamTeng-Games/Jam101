using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.ChangeName)]
    public partial class ChangeNamePanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.ChangeName;

        [SerializeField] private Transform _node_name_exist;
        [SerializeField] private Button _btn_close;
        [SerializeField] private TMP_InputField _input_name;
        [SerializeField] private Transform _node_tip;
        [SerializeField] private Button _btn_ok;

        private void OnValidate()
        {
            _node_name_exist = transform.Find("node_name_exist").GetComponent<Transform>();
            _btn_close = transform.Find("Popup/btn_close").GetComponent<Button>();
            _input_name = transform.Find("Popup/input_name").GetComponent<TMP_InputField>();
            _node_tip = transform.Find("Popup/node_tip").GetComponent<Transform>();
            _btn_ok = transform.Find("Popup/btn_ok").GetComponent<Button>();
        }
    }

}