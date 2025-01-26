using Obvious.Soap;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.EventSystems;
using Input = Quantum.Input;

namespace Jam.Runtime.UI_
{

    // public enum JoystickType
    // {
    //     Fixed,
    //     Follow,
    //     Floating,
    // }

    public partial class JoystickPanel
    {
        [SerializeField] private float _radius = 180;
        [SerializeField] private JoystickType _settingType = JoystickType.Fixed;
        private JoystickType _currentType;
        private Vector3 _originPos;
        
        public override void OnInit()
        {
            _img_touch.AddEventListener(EventTriggerType.PointerDown, OnPointDown);
            _img_touch.AddEventListener(EventTriggerType.PointerUp, OnPointUp);
            _img_touch.AddEventListener(EventTriggerType.Drag, OnPointDrag);
            _originPos = _img_bounds.transform.localPosition;
        }
        
        public override void OnOpen(object userData)
        {
            _currentType = _settingType;
            ResetHandle();
        }
        
        public override void OnClose()
        {
            QuantumCallback.UnsubscribeListener(this);
        }
        
        protected override void OnTick(float dt)
        {
            if (_currentType != _settingType)
            {
                _currentType = _settingType;
                ResetHandle();
            }
        }
        
        private void OnPointDown(BaseEventData obj)
        {
            if (_currentType != JoystickType.Fixed)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_img_touch.rectTransform,
                                                                        ((PointerEventData)obj).position,
                                                                        ((PointerEventData)obj).pressEventCamera,
                                                                        out var localPoint);
                _img_bounds.transform.localPosition = localPoint;
            }
        }
        
        private void OnPointUp(BaseEventData obj)
        {
            ResetHandle();
        }
        
        private void OnPointDrag(BaseEventData obj)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_img_bounds.rectTransform,
                                                                    ((PointerEventData)obj).position,
                                                                    ((PointerEventData)obj).pressEventCamera,
                                                                    out var localPoint);
            // 更新位置
            _img_handle.transform.localPosition = localPoint;
            if (localPoint.magnitude > _radius)
            {
                if (_currentType == JoystickType.Follow)
                {
                    Vector2 offset = localPoint - localPoint.normalized * _radius;
                    _img_bounds.rectTransform.localPosition += new Vector3(offset.x, offset.y);
                }
                _img_handle.transform.localPosition = localPoint.normalized * _radius;
            }
            Debug.Log($"Value {_img_handle.transform.localPosition / _radius}");
            // _input.Value = _img_handle.transform.localPosition / _radius;
        }
        
        private void ResetHandle()
        {
            // _input.ResetValue();
            // _img_handle.rectTransform.localPosition = Vector2.zero;
            _img_handle.transform.localPosition = new Vector3(0, 0, 0);
            _img_bounds.transform.localPosition = _originPos;
        }
    }

}