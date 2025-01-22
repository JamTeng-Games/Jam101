using System;
using Jam.Runtime.Data_;

namespace Jam.Runtime.UI_
{

    public partial class SimpleRoomCellWidget
    {
        private RoomInfo _roomInfo;
        private Action<SimpleRoomCellWidget> _clickAction;

        public RoomInfo RoomInfo => _roomInfo;

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_join.onClick.AddListener(OnClickJoin);
            _roomInfo = userData as RoomInfo;
            Refresh(_roomInfo);
        }

        public override void OnClose()
        {
            _btn_join.onClick.RemoveListener(OnClickJoin);
        }

        protected override void OnTick(float dt)
        {
        }

        public void Refresh(RoomInfo roomInfo)
        {
            _txt_room_name.text = roomInfo.name;
        }

        public void SetClickEvent(Action<SimpleRoomCellWidget> action)
        {
            _clickAction = action;
        }

        private void OnClickJoin()
        {
            _clickAction?.Invoke(this);
        }
    }

}