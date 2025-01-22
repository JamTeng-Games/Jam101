using Jam.Cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIWidget(UIWidgetId.SimpleRoomCell)]
    public partial class SimpleRoomCellWidget : UIWidget
    {
        public override UIWidgetId TypeId => UIWidgetId.SimpleRoomCell;

        [SerializeField] private TextMeshProUGUI _txt_room_name;
        [SerializeField] private Button _btn_join;

        private void OnValidate()
        {
            _txt_room_name = transform.Find("txt_room_name").GetComponent<TextMeshProUGUI>();
            _btn_join = transform.Find("btn_join").GetComponent<Button>();
        }
    }

}