using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.Setting)]
    public partial class SettingPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.Setting;

        [SerializeField] private Button _btn_close;
        [SerializeField] private Button _btn_logout;

        private void OnValidate()
        {
            _btn_close = transform.Find("Popup/btn_close").GetComponent<Button>();
            _btn_logout = transform.Find("Popup/Group_Right/Button_List/btn_logout").GetComponent<Button>();
        }
    }

}