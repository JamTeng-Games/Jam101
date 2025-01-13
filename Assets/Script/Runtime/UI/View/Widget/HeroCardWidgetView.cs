using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIWidget(UIWidgetId.HeroCard)]
    public partial class HeroCardWidget : UIWidget
    {
        public override UIWidgetId TypeId => UIWidgetId.HeroCard;

        [SerializeField] private Image _img_quality;
        [SerializeField] private TextMeshProUGUI _txt_name;
        [SerializeField] private Image _img_quality_glow;
        [SerializeField] private Image _img_hero;
        [SerializeField] private Image _img_quality_gradient;
        [SerializeField] private Button _btn_hero;

        private void OnValidate()
        {
            _img_quality = transform.Find("img_quality").GetComponent<Image>();
            _txt_name = transform.Find("txt_name").GetComponent<TextMeshProUGUI>();
            _img_quality_glow = transform.Find("Mask/img_quality_glow").GetComponent<Image>();
            _img_hero = transform.Find("Mask/img_hero").GetComponent<Image>();
            _img_quality_gradient = transform.Find("Mask/img_quality_gradient").GetComponent<Image>();
            _btn_hero = transform.Find("btn_hero").GetComponent<Button>();
        }
    }

}