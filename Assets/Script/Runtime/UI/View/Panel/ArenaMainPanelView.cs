using cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.ArenaMain)]
    public partial class ArenaMainPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.ArenaMain;

        [SerializeField] private Button _btn_disconnect;

        private void OnValidate()
        {
            _btn_disconnect = transform.Find("btn_disconnect").GetComponent<Button>();
        }
    }

}