using Obvious.Soap;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.EventSystems;
using Input = Quantum.Input;

namespace Jam.Runtime.UI_
{

    public enum JoystickType
    {
        Fixed,
        Follow,
        Floating,
    }

    public partial class JoystickPanel
    {
        [SerializeField] private float _radius = 110;
        [SerializeField] private Vector2Variable _input;
        [SerializeField] private BoolVariable _inputSkill0;
        [SerializeField] private JoystickType _settingType = JoystickType.Fixed;
        private JoystickType _currentType;
        private Vector3 _originPos;

        public override void OnInit()
        {
            Debug.Log("OnInit");
            _img_touch.AddEventListener(EventTriggerType.PointerDown, OnPointDown);
            _img_touch.AddEventListener(EventTriggerType.PointerUp, OnPointUp);
            _img_touch.AddEventListener(EventTriggerType.Drag, OnPointDrag);
            _btn_skill0.onClick.AddListener(OnSkill0Click);
            _btn_change_mode.onClick.AddListener(OnChangeMode);
            _originPos = _img_bounds.transform.localPosition;

            // _input = Game.Input.Joystick;
            // _inputSkill0 = Game.Input.Skill_0;
        }

        private void OnChangeMode()
        {
            _settingType = (JoystickType)(((int)_settingType + 1) % 3);
        }

        private void OnSkill0Click()
        {
            _inputSkill0.Value = true;
        }

        public override void OnOpen(object userData)
        {
            _currentType = _settingType;
            ResetHandle();

            QuantumCallback.Subscribe<CallbackPollInput>(this, PollInput);
        }

        public void PollInput(CallbackPollInput callback)
        {
            // Quantum.Input i = new Quantum.Input();
            // // i.Direction = _input.Value.ToFPVector2();
            // // i.Attack = _inputSkill0.Value;
            // callback.SetInput(i, DeterministicInputFlags.Repeatable);
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
            _input.Value = _img_handle.transform.localPosition / _radius;
        }

        private void ResetHandle()
        {
            _input.ResetValue();
            _img_handle.transform.localPosition = Vector2.zero;
            _img_bounds.transform.localPosition = _originPos;
        }
    }

}