using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIWidget(UIWidgetId.ShopGoods)]
    public partial class ShopGoodsWidget : UIWidget
    {
        public override UIWidgetId TypeId => UIWidgetId.ShopGoods;

        [SerializeField] private Image _img_quality;
        [SerializeField] private TextMeshProUGUI _txt_desc;
        [SerializeField] private Button _btn_lock;
        [SerializeField] private Transform _node_locked;
        [SerializeField] private Transform _node_unlocked;
        [SerializeField] private Button _btn_buy;
        [SerializeField] private TextMeshProUGUI _txt_price;
        [SerializeField] private Image _img_item_icon;
        [SerializeField] private TextMeshProUGUI _txt_name;
        [SerializeField] private TextMeshProUGUI _txt_type;
        [SerializeField] private Transform _node_sold;

        private void OnValidate()
        {
            _img_quality = transform.Find("img_quality").GetComponent<Image>();
            _txt_desc = transform.Find("txt_desc").GetComponent<TextMeshProUGUI>();
            _btn_lock = transform.Find("btn_lock").GetComponent<Button>();
            _node_locked = transform.Find("btn_lock/node_locked").GetComponent<Transform>();
            _node_unlocked = transform.Find("btn_lock/node_unlocked").GetComponent<Transform>();
            _btn_buy = transform.Find("btn_buy").GetComponent<Button>();
            _txt_price = transform.Find("btn_buy/txt_price").GetComponent<TextMeshProUGUI>();
            _img_item_icon = transform.Find("img_item_icon").GetComponent<Image>();
            _txt_name = transform.Find("img_item_icon/txt_name").GetComponent<TextMeshProUGUI>();
            _txt_type = transform.Find("img_item_icon/txt_type").GetComponent<TextMeshProUGUI>();
            _node_sold = transform.Find("node_sold").GetComponent<Transform>();
        }
    }

}