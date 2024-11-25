using UnityEngine;
using UnityEngine.UI;

namespace Jam.Runtime.UI_
{

    [RequireComponent(typeof(Slider))]
    public class JSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private void OnValidate()
        {
            _slider = GetComponent<Slider>();
        }
    }

}