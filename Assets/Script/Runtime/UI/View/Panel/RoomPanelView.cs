using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.Room)]
    public partial class RoomPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.Room;

        [SerializeField] private Button _btn_back;
        [SerializeField] private Button _btn_home;
        [SerializeField] private TextMeshProUGUI _txt_room_name;
        [SerializeField] private Transform _node_content;
        [SerializeField] private Button _btn_ready;
        [SerializeField] private TextMeshProUGUI _txt_ready;
        [SerializeField] private Button _btn_battle;
        [SerializeField] private Button _btn_chat;
        [SerializeField] private TMP_InputField _input_chat;

        private void OnValidate()
        {
            _btn_back = transform.Find("Top/btn_back").GetComponent<Button>();
            _btn_home = transform.Find("Top/btn_home").GetComponent<Button>();
            _txt_room_name = transform.Find("Top/txt_room_name").GetComponent<TextMeshProUGUI>();
            _node_content = transform.Find("ScrollRect/node_content").GetComponent<Transform>();
            _btn_ready = transform.Find("btn_ready").GetComponent<Button>();
            _txt_ready = transform.Find("btn_ready/txt_ready").GetComponent<TextMeshProUGUI>();
            _btn_battle = transform.Find("btn_battle").GetComponent<Button>();
            _btn_chat = transform.Find("btn_chat").GetComponent<Button>();
            _input_chat = transform.Find("input_chat").GetComponent<TMP_InputField>();
        }
    }

}