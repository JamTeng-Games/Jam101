using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIWidget(UIWidgetId.Money)]
    public partial class MoneyWidget : UIWidget
    {
        public override UIWidgetId TypeId => UIWidgetId.Money;

        [SerializeField] private Transform _node_energy;
        [SerializeField] private TextMeshProUGUI _txt_energy;
        [SerializeField] private Transform _node_gold;
        [SerializeField] private TextMeshProUGUI _txt_gold;
        [SerializeField] private Transform _node_gem;
        [SerializeField] private TextMeshProUGUI _txt_gem;

        private void OnValidate()
        {
            _node_energy = transform.Find("node_energy").GetComponent<Transform>();
            _txt_energy = transform.Find("node_energy/txt_energy").GetComponent<TextMeshProUGUI>();
            _node_gold = transform.Find("node_gold").GetComponent<Transform>();
            _txt_gold = transform.Find("node_gold/txt_gold").GetComponent<TextMeshProUGUI>();
            _node_gem = transform.Find("node_gem").GetComponent<Transform>();
            _txt_gem = transform.Find("node_gem/txt_gem").GetComponent<TextMeshProUGUI>();
        }
    }

}