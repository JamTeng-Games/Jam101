using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIWidget(UIWidgetId.Joystick)]
    public partial class JoystickWidget : UIWidget
    {
        public override UIWidgetId TypeId => UIWidgetId.Joystick;

        [SerializeField] private Image _img_bk;
        [SerializeField] private Button _btn_start;

        private void OnValidate()
        {
            _img_bk = transform.Find("img_bk").GetComponent<Image>();
            _btn_start = transform.Find("btn_start").GetComponent<Button>();
        }
    }

}