using cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIWidget(UIWidgetId.HeroCard)]
    public partial class HeroCardWidget : UIWidget
    {
        public override UIWidgetId TypeId => UIWidgetId.HeroCard;

        [SerializeField] private Image _img_bg;
        [SerializeField] private TextMeshProUGUI _txt_name;
        [SerializeField] private Image _img_hero_icon;

        private void OnValidate()
        {
            _img_bg = transform.Find("img_bg").GetComponent<Image>();
            _txt_name = transform.Find("txt_name").GetComponent<TextMeshProUGUI>();
            _img_hero_icon = transform.Find("Mask/img_hero_icon").GetComponent<Image>();
        }
    }

}