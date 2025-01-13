using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.Back)]
    public partial class BackPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.Back;

        [SerializeField] private Button _btn_back;

        private void OnValidate()
        {
            _btn_back = transform.Find("btn_back").GetComponent<Button>();
        }
    }

}