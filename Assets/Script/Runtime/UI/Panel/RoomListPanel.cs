using System.Collections.Generic;
using Jam.Cfg;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;
using Jam.Runtime.Net_;
using TMPro;

namespace Jam.Runtime.UI_
{

    public partial class RoomListPanel
    {
        private List<RoomInfo> _rooms;
        private List<int> _widgetIds = new List<int>();

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_close.onClick.AddListener(OnClickClose);
            _btn_create.onClick.AddListener(OnClickCreate);
            G.Event.Subscribe<RoomListRes>(GlobalEventId.RoomListUpdate, OnRoomListUpdate);
        }

        public override void OnClose()
        {
            _btn_close.onClick.RemoveListener(OnClickClose);
            _btn_create.onClick.RemoveListener(OnClickCreate);
            G.Event.Unsubscribe<RoomListRes>(GlobalEventId.RoomListUpdate, OnRoomListUpdate);

            this.ClearWidgets();
            _widgetIds.Clear();
        }

        protected override void OnTick(float dt)
        {
        }

        public override void OnShow()
        {
            // req room list
            G.Net.Send(NetCmd.CS_QueryRoomList, EmptyMsg.Data);
        }

        private void OnClickClose()
        {
            G.UI.Back();
        }

        private void OnClickCreate()
        {
            if (string.IsNullOrEmpty(_input_name.text))
                return;

            G.Net.Send(NetCmd.CS_CreateRoom, new CreateRoomReq() { name = _input_name.text });
            G.UI.Back();
        }

        private void OnRoomListUpdate(RoomListRes obj)
        {
            _rooms = obj.rooms;
            RefreshRoomList();
        }

        private void RefreshRoomList()
        {
            // 隐藏多余的
            if (_widgetIds.Count > _rooms.Count)
            {
                for (int i = _rooms.Count; i < _widgetIds.Count; i++)
                {
                    var widget = this.GetWidget<SimpleRoomCellWidget>(_widgetIds[i]);
                    widget.gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < _rooms.Count; i++)
            {
                var roomInfo = _rooms[i];
                if (_widgetIds.Count <= i)
                {
                    var widgetId = this.AddWidget(UIWidgetId.SimpleRoomCell, _node_content, w =>
                    {
                        (w as SimpleRoomCellWidget).SetClickEvent(OnClickJoinRoom);
                    }, userData: roomInfo);
                    _widgetIds.Add(widgetId);
                }
                else
                {
                    var widget = this.GetWidget<SimpleRoomCellWidget>(_widgetIds[i]);
                    widget?.gameObject.SetActive(true);
                    widget?.Refresh(roomInfo);
                }
            }
        }

        private void OnClickJoinRoom(SimpleRoomCellWidget widget)
        {
            var roomInfo = widget.RoomInfo;
            G.UI.Back();
            G.Net.Send(NetCmd.CS_JoinRoom, new JoinRoomReq() { id = roomInfo.id });
        }
    }

}