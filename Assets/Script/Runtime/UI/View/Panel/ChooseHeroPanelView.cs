using cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.ChooseHero)]
    public partial class ChooseHeroPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.ChooseHero;

        [SerializeField] private Transform _node_heros;
        [SerializeField] private Button _btn_confirm;

        private void OnValidate()
        {
            _node_heros = transform.Find("node_heros").GetComponent<Transform>();
            _btn_confirm = transform.Find("btn_confirm").GetComponent<Button>();
        }
    }

}