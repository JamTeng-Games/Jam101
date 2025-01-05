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

        [SerializeField] private Button _btn_inventory;
        [SerializeField] private Button _btn_battle;
        [SerializeField] private Button _btn_shop;
        [SerializeField] private Button _btn_settings;
        [SerializeField] private TextMeshProUGUI _txt_player_level;
        [SerializeField] private TextMeshProUGUI _txt_player_name;
        [SerializeField] private Image _img_hero;

        private void OnValidate()
        {
            _btn_inventory = transform.Find("Bottom/ManMenu/btn_inventory").GetComponent<Button>();
            _btn_battle = transform.Find("Bottom/btn_battle").GetComponent<Button>();
            _btn_shop = transform.Find("Bottom/btn_shop").GetComponent<Button>();
            _btn_settings = transform.Find("btn_settings").GetComponent<Button>();
            _txt_player_level = transform.Find("User_Info/Level_Frame/txt_player_level").GetComponent<TextMeshProUGUI>();
            _txt_player_name = transform.Find("User_Info/txt_player_name").GetComponent<TextMeshProUGUI>();
            _img_hero = transform.Find("img_hero").GetComponent<Image>();
        }
    }

}