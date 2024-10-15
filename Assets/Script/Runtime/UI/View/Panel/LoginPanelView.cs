using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace J.Runtime.UI
{
    public partial class LoginPanel
    {
        [SerializeField] private TMP_InputField _input_account;
        [SerializeField] private TMP_InputField _input_password;
        [SerializeField] private Button _btn_login;
    }
}