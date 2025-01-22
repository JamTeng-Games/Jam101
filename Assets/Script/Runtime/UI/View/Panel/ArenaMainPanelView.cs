using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.ArenaMain)]
    public partial class ArenaMainPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.ArenaMain;

        [SerializeField] private TextMeshProUGUI _txt_time;
        [SerializeField] private Transform _node_kill1;
        [SerializeField] private Transform _node_kill2;
        [SerializeField] private Transform _node_kill3;
        [SerializeField] private Transform _node_kill4;
        [SerializeField] private Transform _node_kill5;
        [SerializeField] private Button _btn_disconnect;

        private void OnValidate()
        {
            _txt_time = transform.Find("Title_Ribbon_Blue/txt_time").GetComponent<TextMeshProUGUI>();
            _node_kill1 = transform.Find("kill_tip/node_kill1").GetComponent<Transform>();
            _node_kill2 = transform.Find("kill_tip/node_kill2").GetComponent<Transform>();
            _node_kill3 = transform.Find("kill_tip/node_kill3").GetComponent<Transform>();
            _node_kill4 = transform.Find("kill_tip/node_kill4").GetComponent<Transform>();
            _node_kill5 = transform.Find("kill_tip/node_kill5").GetComponent<Transform>();
            _btn_disconnect = transform.Find("btn_disconnect").GetComponent<Button>();
        }
    }

}