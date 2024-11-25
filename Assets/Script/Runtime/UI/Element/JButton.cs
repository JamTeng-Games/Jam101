using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Runtime.UI_
{

    [RequireComponent(typeof(Button))]
    public class JButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void OnValidate()
        {
            _button = GetComponent<Button>();
        }
    }

}