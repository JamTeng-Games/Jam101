using System.Collections.Generic;
using Jam.Cfg;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;
using Jam.Runtime.Helpers;
using Jam.Runtime.Net_;

namespace Jam.Runtime.UI_
{

    public partial class RoomPanel
    {
        private bool _isReady;
        private List<int> _widgetIds = new List<int>();

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_back.onClick.AddListener(OnClickBack);
            _btn_exit.onClick.AddListener(OnClickExit);
            _btn_ready.onClick.AddListener(OnClickReady);
            _btn_chat.onClick.AddListener(OnClickChat);
            _btn_battle.onClick.AddListener(OnClickBattle);
            G.Event.Subscribe<RoomUser>(GlobalEventId.RoomUserEnter, OnRoomUserEnter);
            G.Event.Subscribe<RoomSeat>(GlobalEventId.RoomUserLeave, OnRoomUserLeave);
            G.Event.Subscribe<RoomUserChat>(GlobalEventId.RoomUserChat, OnRoomUserChat);
            G.Event.Subscribe<RoomSeatReady>(GlobalEventId.RoomUserReadyUpdate, OnRoomUserReadyUpdate);

            _isReady = RoomHelper.IsSelfReady();

            InitWidgets();
            // Refresh();
            RefreshButton();
            _txt_room_name.text = G.Data.RoomData.name;
        }

        public override void OnClose()
        {
            _btn_back.onClick.RemoveListener(OnClickBack);
            _btn_exit.onClick.RemoveListener(OnClickExit);
            _btn_ready.onClick.RemoveListener(OnClickReady);
            _btn_chat.onClick.RemoveListener(OnClickChat);
            _btn_battle.onClick.RemoveListener(OnClickBattle);
            G.Event.Unsubscribe<RoomUser>(GlobalEventId.RoomUserEnter, OnRoomUserEnter);
            G.Event.Unsubscribe<RoomSeat>(GlobalEventId.RoomUserLeave, OnRoomUserLeave);
            G.Event.Unsubscribe<RoomUserChat>(GlobalEventId.RoomUserChat, OnRoomUserChat);
            G.Event.Unsubscribe<RoomSeatReady>(GlobalEventId.RoomUserReadyUpdate, OnRoomUserReadyUpdate);

            this.ClearWidgets();
            _widgetIds.Clear();
        }

        protected override void OnTick(float dt)
        {
        }

        private void InitWidgets()
        {
            // 8个座位
            for (int i = 0; i < 8; i++)
            {
                RoomUser user = RoomHelper.GetRoomUserBySeat(i + 1);
                var widgetId = this.AddWidget(UIWidgetId.SimpleSeat, _node_content, userData: user);
                _widgetIds.Add(widgetId);
            }
        }

        private void Refresh()
        {
            // ResetAllSeats();
            var users = G.Data.RoomData.users;
            foreach (var user in users)
            {
                var widgetId = _widgetIds[user.seat - 1];
                var widget = this.GetWidget<SimpleSeatWidget>(widgetId);
                widget?.Refresh(user);
            }
        }

        private void OnRoomUserEnter(RoomUser info)
        {
            var widgetId = _widgetIds[info.seat - 1];
            var widget = this.GetWidget<SimpleSeatWidget>(widgetId);
            widget?.Refresh(info);
        }

        private void OnRoomUserLeave(RoomSeat info)
        {
            var widgetId = _widgetIds[info.seat - 1];
            var widget = this.GetWidget<SimpleSeatWidget>(widgetId);
            widget?.Refresh(null);
        }

        private void OnRoomUserChat(RoomUserChat info)
        {
            var widgetId = _widgetIds[info.seat - 1];
            var widget = this.GetWidget<SimpleSeatWidget>(widgetId);
            widget?.ShowChat(info.msg);
        }

        private void OnClickBack()
        {
            G.UI.Back();
        }

        private void OnClickExit()
        {
            G.Net.Send(NetCmd.CS_LeaveRoom, EmptyMsg.Data);
        }

        private void OnClickChat()
        {
            if (string.IsNullOrEmpty(_input_chat.text))
                return;

            G.Net.Send(NetCmd.CS_RoomChat, new RoomChatReq() { msg = _input_chat.text });
            _input_chat.text = string.Empty;
        }

        private void OnClickReady()
        {
            if (G.Data.RoomData.seat != 1)
            {
                G.Net.Send(NetCmd.CS_RoomUserReady, new RoomUserReadyReq() { ready = !_isReady });
            }
        }

        private void OnClickBattle()
        {
            G.Net.Send(NetCmd.CS_RoomStartBattle, EmptyMsg.Data);
        }

        // private void ResetAllSeats()
        // {
        //     // 8个座位
        //     for (int i = 0; i < 8; i++)
        //     {
        //         var widgetId = _widgetIds[i];
        //         var widget = this.GetWidget<SimpleSeatWidget>(widgetId);
        //         widget?.Reset();
        //     }
        // }

        private void RefreshButton()
        {
            // 房主
            if (G.Data.RoomData.seat == 1)
            {
                if (RoomHelper.AllSeatReady())
                {
                    _btn_battle.gameObject.SetActive(true);
                }
                else
                {
                    _btn_battle.gameObject.SetActive(false);
                    _txt_ready.text = "Waiting";
                }
            }
        }

        private void OnRoomUserReadyUpdate(RoomSeatReady obj)
        {
            if (obj.seat == G.Data.RoomData.seat)
            {
                _isReady = obj.ready;
                _txt_ready.text = _isReady ? "Cancel" : "Ready";
            }

            var widgetId = _widgetIds[obj.seat - 1];
            var widget = this.GetWidget<SimpleSeatWidget>(widgetId);
            widget?.Ready(obj.ready);

            RefreshButton();
        }
    }

}