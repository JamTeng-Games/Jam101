using cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.Home)]
    public partial class HomePanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.Home;

        [SerializeField] private Button _btn_shop;
        [SerializeField] private Button _btn_inventory;
        [SerializeField] private Button _btn_battle;
        [SerializeField] private Button _btn_settings;
        [SerializeField] private Slider _slider_exp;
        [SerializeField] private TextMeshProUGUI _txt_exp_desc;
        [SerializeField] private TextMeshProUGUI _txt_player_level;
        [SerializeField] private TextMeshProUGUI _txt_player_name;

        private void OnValidate()
        {
            _btn_shop = transform.Find("Bottom/ManMenu/btn_shop").GetComponent<Button>();
            _btn_inventory = transform.Find("Bottom/ManMenu/btn_inventory").GetComponent<Button>();
            _btn_battle = transform.Find("Bottom/btn_battle").GetComponent<Button>();
            _btn_settings = transform.Find("btn_settings").GetComponent<Button>();
            _slider_exp = transform.Find("User_Info/slider_exp").GetComponent<Slider>();
            _txt_exp_desc = transform.Find("User_Info/slider_exp/Bg/txt_exp_desc").GetComponent<TextMeshProUGUI>();
            _txt_player_level = transform.Find("User_Info/Level_Frame/txt_player_level").GetComponent<TextMeshProUGUI>();
            _txt_player_name = transform.Find("User_Info/txt_player_name").GetComponent<TextMeshProUGUI>();
        }
    }

}