using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.RoomList)]
    public partial class RoomListPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.RoomList;

        [SerializeField] private Button _btn_close;
        [SerializeField] private Button _btn_create;
        [SerializeField] private Transform _node_content;
        [SerializeField] private TMP_InputField _input_name;

        private void OnValidate()
        {
            _btn_close = transform.Find("Popup/btn_close").GetComponent<Button>();
            _btn_create = transform.Find("Popup/btn_create").GetComponent<Button>();
            _node_content = transform.Find("Popup/ScrollRect/node_content").GetComponent<Transform>();
            _input_name = transform.Find("input_name").GetComponent<TMP_InputField>();
        }
    }

}