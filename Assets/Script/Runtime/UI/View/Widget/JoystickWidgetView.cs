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

        [SerializeField] private Image _img_touch;
        [SerializeField] private Image _img_bounds;
        [SerializeField] private Image _img_handle;

        private void OnValidate()
        {
            _img_touch = transform.Find("img_touch").GetComponent<Image>();
            _img_bounds = transform.Find("img_touch/img_bounds").GetComponent<Image>();
            _img_handle = transform.Find("img_touch/img_bounds/img_handle").GetComponent<Image>();
        }
    }

}