using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIWidget(UIWidgetId.ItemInfo)]
    public partial class ItemInfoWidget : UIWidget
    {
        public override UIWidgetId TypeId => UIWidgetId.ItemInfo;

        [SerializeField] private Image _img_bg;
        [SerializeField] private Transform _node_info;
        [SerializeField] private TextMeshProUGUI _txt_quality;
        [SerializeField] private TextMeshProUGUI _txt_name;
        [SerializeField] private TextMeshProUGUI _txt_desc;
        [SerializeField] private Transform _node_attrib;
        [SerializeField] private Image _img_icon_quality;
        [SerializeField] private Image _img_icon;
        [SerializeField] private Transform _node_button;
        [SerializeField] private Button _btn_sell;
        [SerializeField] private Image _img_money_icon;
        [SerializeField] private TextMeshProUGUI _txt_sell_price;
        [SerializeField] private Transform _node_empty_info;

        private void OnValidate()
        {
            _img_bg = transform.Find("img_bg").GetComponent<Image>();
            _node_info = transform.Find("node_info").GetComponent<Transform>();
            _txt_quality = transform.Find("node_info/txt_quality").GetComponent<TextMeshProUGUI>();
            _txt_name = transform.Find("node_info/txt_name").GetComponent<TextMeshProUGUI>();
            _txt_desc = transform.Find("node_info/txt_desc").GetComponent<TextMeshProUGUI>();
            _node_attrib = transform.Find("node_info/node_attrib").GetComponent<Transform>();
            _img_icon_quality = transform.Find("node_info/img_icon_quality").GetComponent<Image>();
            _img_icon = transform.Find("node_info/img_icon_quality/img_icon").GetComponent<Image>();
            _node_button = transform.Find("node_info/node_button").GetComponent<Transform>();
            _btn_sell = transform.Find("node_info/node_button/btn_sell").GetComponent<Button>();
            _img_money_icon = transform.Find("node_info/node_button/btn_sell/Gold/img_money_icon").GetComponent<Image>();
            _txt_sell_price = transform.Find("node_info/node_button/btn_sell/Gold/txt_sell_price").GetComponent<TextMeshProUGUI>();
            _node_empty_info = transform.Find("node_empty_info").GetComponent<Transform>();
        }
    }

}