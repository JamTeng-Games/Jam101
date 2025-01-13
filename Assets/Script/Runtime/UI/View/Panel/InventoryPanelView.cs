using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.Inventory)]
    public partial class InventoryPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.Inventory;

        [SerializeField] private Button _btn_back;
        [SerializeField] private Transform _node_money;
        [SerializeField] private Button _btn_home;
        [SerializeField] private Transform _node_content;
        [SerializeField] private Transform _node_iteminfo;

        private void OnValidate()
        {
            _btn_back = transform.Find("Top/btn_back").GetComponent<Button>();
            _node_money = transform.Find("Top/node_money").GetComponent<Transform>();
            _btn_home = transform.Find("Top/btn_home").GetComponent<Button>();
            _node_content = transform.Find("Group_Left/ScrollRect/node_content").GetComponent<Transform>();
            _node_iteminfo = transform.Find("Group_Right/node_iteminfo").GetComponent<Transform>();
        }
    }

}