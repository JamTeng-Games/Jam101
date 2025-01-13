using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.HeroList)]
    public partial class HeroListPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.HeroList;

        [SerializeField] private Button _btn_back;
        [SerializeField] private Button _btn_home;
        [SerializeField] private Transform _node_content;

        private void OnValidate()
        {
            _btn_back = transform.Find("Top/btn_back").GetComponent<Button>();
            _btn_home = transform.Find("Top/btn_home").GetComponent<Button>();
            _node_content = transform.Find("ScrollRect/node_content").GetComponent<Transform>();
        }
    }

}