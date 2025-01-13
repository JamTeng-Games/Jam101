using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIWidget(UIWidgetId.ItemCell)]
    public partial class ItemCellWidget : UIWidget
    {
        public override UIWidgetId TypeId => UIWidgetId.ItemCell;

        [SerializeField] private Image _img_empty;
        [SerializeField] private Image _img_quality;
        [SerializeField] private Image _img_icon;
        [SerializeField] private TextMeshProUGUI _txt_Count;
        [SerializeField] private Transform _node_select;

        private void OnValidate()
        {
            _img_empty = transform.Find("img_empty").GetComponent<Image>();
            _img_quality = transform.Find("img_quality").GetComponent<Image>();
            _img_icon = transform.Find("img_icon").GetComponent<Image>();
            _txt_Count = transform.Find("txt_Count").GetComponent<TextMeshProUGUI>();
            _node_select = transform.Find("node_select").GetComponent<Transform>();
        }
    }

}