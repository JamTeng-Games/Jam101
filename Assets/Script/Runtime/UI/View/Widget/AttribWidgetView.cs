using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIWidget(UIWidgetId.Attrib)]
    public partial class AttribWidget : UIWidget
    {
        public override UIWidgetId TypeId => UIWidgetId.Attrib;

        [SerializeField] private Image _img_bg;
        [SerializeField] private TextMeshProUGUI _txt_icon;
        [SerializeField] private TextMeshProUGUI _txt_name;
        [SerializeField] private TextMeshProUGUI _txt_value;

        private void OnValidate()
        {
            _img_bg = transform.Find("img_bg").GetComponent<Image>();
            _txt_icon = transform.Find("txt_icon").GetComponent<TextMeshProUGUI>();
            _txt_name = transform.Find("txt_name").GetComponent<TextMeshProUGUI>();
            _txt_value = transform.Find("txt_value").GetComponent<TextMeshProUGUI>();
        }
    }

}