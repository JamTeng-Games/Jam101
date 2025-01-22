using Jam.Runtime.Data_;

namespace Jam.Runtime.UI_
{

    public partial class SimpleSeatWidget
    {
        private float _chatTimer = 0f;
        private float _chatDuration = 6f;
        private bool _isChatShow = false;

        public override void OnInit()
        {
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
            _img_bg.color = UnityEngine.Color.blue;
        }

        public void Ready(bool isReady)
        {
            if (isReady)
            {
                _img_empty.gameObject.SetActive(false);
                _img_bg.color = UnityEngine.Color.green;
            }
            else
            {
                _img_bg.color = UnityEngine.Color.blue;
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