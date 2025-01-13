using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.Joystick)]
    public partial class JoystickPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.Joystick;

        [SerializeField] private Image _img_touch;
        [SerializeField] private Image _img_bounds;
        [SerializeField] private Image _img_handle;
        [SerializeField] private Button _btn_change_mode;
        [SerializeField] private Button _btn_skill0;

        private void OnValidate()
        {
            _img_touch = transform.Find("img_touch").GetComponent<Image>();
            _img_bounds = transform.Find("img_touch/img_bounds").GetComponent<Image>();
            _img_handle = transform.Find("img_touch/img_bounds/img_handle").GetComponent<Image>();
            _btn_change_mode = transform.Find("btn_change_mode").GetComponent<Button>();
            _btn_skill0 = transform.Find("skills/btn_skill0").GetComponent<Button>();
        }
    }

}