using cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.Money)]
    public partial class MoneyPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.Money;

        [SerializeField] private Transform _node_container;
        [SerializeField] private Transform _node_energy;
        [SerializeField] private Button _btn_energy_add;
        [SerializeField] private TextMeshProUGUI _txt_energy_num;
        [SerializeField] private Transform _node_gold;
        [SerializeField] private Button _btn_gold_add;
        [SerializeField] private TextMeshProUGUI _txt_gold_num;
        [SerializeField] private Transform _node_gem;
        [SerializeField] private Button _btn_gem_add;
        [SerializeField] private TextMeshProUGUI _txt_gem_num;

        private void OnValidate()
        {
            _node_container = transform.Find("node_container").GetComponent<Transform>();
            _node_energy = transform.Find("node_container/node_energy").GetComponent<Transform>();
            _btn_energy_add = transform.Find("node_container/node_energy/btn_energy_add").GetComponent<Button>();
            _txt_energy_num = transform.Find("node_container/node_energy/txt_energy_num").GetComponent<TextMeshProUGUI>();
            _node_gold = transform.Find("node_container/node_gold").GetComponent<Transform>();
            _btn_gold_add = transform.Find("node_container/node_gold/btn_gold_add").GetComponent<Button>();
            _txt_gold_num = transform.Find("node_container/node_gold/txt_gold_num").GetComponent<TextMeshProUGUI>();
            _node_gem = transform.Find("node_container/node_gem").GetComponent<Transform>();
            _btn_gem_add = transform.Find("node_container/node_gem/btn_gem_add").GetComponent<Button>();
            _txt_gem_num = transform.Find("node_container/node_gem/txt_gem_num").GetComponent<TextMeshProUGUI>();
        }
    }

}