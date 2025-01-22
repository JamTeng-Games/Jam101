using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIWidget(UIWidgetId.SimpleSeat)]
    public partial class SimpleSeatWidget : UIWidget
    {
        public override UIWidgetId TypeId => UIWidgetId.SimpleSeat;

        [SerializeField] private Image _img_bg;
        [SerializeField] private Image _img_empty;
        [SerializeField] private TextMeshProUGUI _txt_name;
        [SerializeField] private Transform _node_chat;
        [SerializeField] private TextMeshProUGUI _txt_chat_msg;
        [SerializeField] private Transform _node_owner;

        private void OnValidate()
        {
            _img_bg = transform.Find("img_bg").GetComponent<Image>();
            _img_empty = transform.Find("img_empty").GetComponent<Image>();
            _txt_name = transform.Find("txt_name").GetComponent<TextMeshProUGUI>();
            _node_chat = transform.Find("node_chat").GetComponent<Transform>();
            _txt_chat_msg = transform.Find("node_chat/txt_chat_msg").GetComponent<TextMeshProUGUI>();
            _node_owner = transform.Find("node_owner").GetComponent<Transform>();
        }
    }

}