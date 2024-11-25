using cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    public partial class StartupPanel : MonoBehaviour
    {
        [SerializeField] private Slider _slider_download_progress;
        [SerializeField] private Transform _node_slider_handle;
        [SerializeField] private TextMeshProUGUI _txt_progress_info;
        [SerializeField] private Transform _node_confirm;
        [SerializeField] private TextMeshProUGUI _txt_confirm_title;
        [SerializeField] private TextMeshProUGUI _txt_confirm_context;
        [SerializeField] private Button _btn_confirm_cancel;
        [SerializeField] private Button _btn_confirm_ok;

        private void OnValidate()
        {
            _slider_download_progress = transform.Find("slider_download_progress").GetComponent<Slider>();
            _node_slider_handle = transform.Find("slider_download_progress/node_slider_handle").GetComponent<Transform>();
            _txt_progress_info = transform.Find("slider_download_progress/node_slider_handle/_Label_Bubble02_White/txt_progress_info").GetComponent<TextMeshProUGUI>();
            _node_confirm = transform.Find("node_confirm").GetComponent<Transform>();
            _txt_confirm_title = transform.Find("node_confirm/Popup/txt_confirm_title").GetComponent<TextMeshProUGUI>();
            _txt_confirm_context = transform.Find("node_confirm/Popup/txt_confirm_context").GetComponent<TextMeshProUGUI>();
            _btn_confirm_cancel = transform.Find("node_confirm/Popup/btn_confirm_cancel").GetComponent<Button>();
            _btn_confirm_ok = transform.Find("node_confirm/Popup/btn_confirm_ok").GetComponent<Button>();
        }
    }

}