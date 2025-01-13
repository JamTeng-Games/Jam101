using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.Shop)]
    public partial class ShopPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.Shop;

        [SerializeField] private Button _btn_back;
        [SerializeField] private TextMeshProUGUI _txt_level;
        [SerializeField] private TextMeshProUGUI _txt_life;
        [SerializeField] private TextMeshProUGUI _txt_gold;
        [SerializeField] private Transform _node_money;
        [SerializeField] private Transform _node_goods_list;
        [SerializeField] private Button _btn_refresh;
        [SerializeField] private TextMeshProUGUI _txt_refresh_price;
        [SerializeField] private Button _btn_stats;
        [SerializeField] private Button _btn_inventory;
        [SerializeField] private Transform _node_inventory;
        [SerializeField] private Button _btn_start_game;

        private void OnValidate()
        {
            _btn_back = transform.Find("Top/btn_back").GetComponent<Button>();
            _txt_level = transform.Find("Top/btn_back/txt_level").GetComponent<TextMeshProUGUI>();
            _txt_life = transform.Find("Top/StatusBar_Group/Stats_life/txt_life").GetComponent<TextMeshProUGUI>();
            _txt_gold = transform.Find("Top/StatusBar_Group/Stats_Gold/txt_gold").GetComponent<TextMeshProUGUI>();
            _node_money = transform.Find("Top/node_money").GetComponent<Transform>();
            _node_goods_list = transform.Find("Middle/node_goods_list").GetComponent<Transform>();
            _btn_refresh = transform.Find("Middle/btn_refresh").GetComponent<Button>();
            _txt_refresh_price = transform.Find("Middle/btn_refresh/txt_refresh_price").GetComponent<TextMeshProUGUI>();
            _btn_stats = transform.Find("Middle/btn_stats").GetComponent<Button>();
            _btn_inventory = transform.Find("Bottom/btn_inventory").GetComponent<Button>();
            _node_inventory = transform.Find("Bottom/player_item_list/Viewport/node_inventory").GetComponent<Transform>();
            _btn_start_game = transform.Find("Bottom/btn_start_game").GetComponent<Button>();
        }
    }

}