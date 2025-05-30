﻿using Jam.Runtime.Data_;
using UnityEngine.Diagnostics;

namespace Jam.Runtime.UI_
{

    public partial class SimpleSeatWidget
    {
        private float _chatTimer = 0f;
        private float _chatDuration = 6f;
        private bool _isChatShow = false;

        private UnityEngine.Color _readyColor;
        private UnityEngine.Color _unreadyColor;

        public override void OnInit()
        {
            _readyColor = Jam.Core.Utils.Color.FromHex("3CB91D");
            _unreadyColor = Jam.Core.Utils.Color.FromHex("1171C0");
        }

        public override void OnOpen(object userData)
        {
            RoomUser user = userData as RoomUser;
            Refresh(user);
        }

        public override void OnClose()
        {
        }

        protected override void OnTick(float dt)
        {
            if (_isChatShow)
            {
                _chatTimer += dt;
                if (_chatTimer > _chatDuration)
                    HideChat();
            }
        }

        public void Refresh(RoomUser user)
        {
            _img_empty.gameObject.SetActive(user == null);
            if (user == null)
            {
                Ready(false);
                SetRoomOwner(false);
                _txt_name.text = "No user";
            }
            else
            {
                _txt_name.text = user.name;
                Ready(user.ready);
                SetRoomOwner(user.seat == 1);
            }
        }

        public void ShowChat(string infoMsg)
        {
            _isChatShow = true;
            _chatTimer = 0f;
            _node_chat.gameObject.SetActive(true);
            _txt_chat_msg.text = infoMsg;
        }

        public void Reset()
        {
            _img_empty.gameObject.SetActive(true);
            _node_chat.gameObject.SetActive(false);
            _img_bg.color = _unreadyColor;
        }

        public void Ready(bool isReady)
        {
            if (isReady)
            {
                _img_empty.gameObject.SetActive(false);
                _img_bg.color = _readyColor;
            }
            else
            {
                _img_bg.color = _unreadyColor;
            }
        }

        private void HideChat()
        {
            _isChatShow = false;
            _node_chat.gameObject.SetActive(false);
        }

        private void SetRoomOwner(bool isOwner)
        {
            _node_owner.gameObject.SetActive(isOwner);
        }
    }

}