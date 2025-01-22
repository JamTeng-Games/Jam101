using UnityEditor;
using UnityEngine;

namespace Jam.Runtime.UI_
{

    public partial class SettingPanel
    {
        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_close.onClick.AddListener(OnClickClose);
            _btn_logout.onClick.AddListener(OnClickLogout);
        }

        public override void OnClose()
        {
            _btn_close.onClick.RemoveListener(OnClickClose);
            _btn_logout.onClick.RemoveListener(OnClickLogout);
        }

        protected override void OnTick(float dt)
        {
        }

        private void OnClickLogout()
        {
            PlayerPrefs.DeleteKey("account");
            PlayerPrefs.DeleteKey("pwd");

#if UNITY_EDITOR
            // 在编辑器模式下停止运行
            EditorApplication.isPlaying = false;
            return;
#endif
            Application.Quit();
        }

        private void OnClickClose()
        {
            G.UI.Back();
        }
    }

}