using Jam.Core;
using Jam.Runtime.Event;
using Jam.Runtime.Quantum_;
using UnityEngine;

namespace Jam.Runtime.UI_
{

    public partial class ArenaMainPanel
    {
        private const float _continueKillTimeout = 10;
        private const float _killTipTimeout = 5;

        private int _killCount = 0;
        private bool _isKilling = false;
        private float _killTimer = 0;

        private bool _killTipShowing = false;
        private float _killTipTimer = 0;

        // Joystics
        private JoystickWidget _moveJoystick;
        private JoystickWidget _attackJoystick;
        private JoystickWidget _skillJoystick;
        private JoystickWidget _superJoystick;

        public override void OnInit()
        {
            InitJoystick();
        }

        public override void OnOpen(object userData)
        {
            OpenJoystick();

#if UNITY_EDITOR
            _btn_disconnect.onClick.AddListener(OnClickDisconnect);
#endif
            G.Event.Subscribe<int>(GlobalEventId.CombatTimeUpdate, OnTimeUpdate);
            G.Event.Subscribe<string>(GlobalEventId.KillHero, OnKillHero);
        }

        public override void OnClose()
        {
#if UNITY_EDITOR
            _btn_disconnect.onClick.RemoveListener(OnClickDisconnect);
#endif
            G.Event.Unsubscribe<int>(GlobalEventId.CombatTimeUpdate, OnTimeUpdate);
            G.Event.Unsubscribe<string>(GlobalEventId.KillHero, OnKillHero);

            CloseJoystick();
        }

        protected override void OnTick(float dt)
        {
            if (_isKilling)
            {
                _killTimer += dt;
                if (_killTimer >= _continueKillTimeout)
                {
                    _isKilling = false;
                    _killTimer = 0;
                    _killCount = 0;
                    HideKillTip();
                }
            }

            if (_killTipShowing)
            {
                _killTipTimer += dt;
                if (_killTipTimer >= _killTipTimeout)
                {
                    _killTipTimer = 0;
                    HideKillTip();
                }
            }

            TickJoystick(dt);
        }

        private void OnClickDisconnect()
        {
            G.Instance.QuantumChannel.DisconnectAsync(QuantumConnectFailReason.UserRequest);
        }

        private void OnTimeUpdate(int time)
        {
            int minute = time / 60;
            int second = time % 60;
            _txt_time.text = $"{minute:D2}:{second:D2}";
        }

        private void OnKillHero(string obj)
        {
            _isKilling = true;
            _killTimer = 0;
            _killCount++;

            ShowKillTip();
        }

        private void ShowKillTip()
        {
            _node_kill1.gameObject.SetActive(_killCount == 1);
            _node_kill2.gameObject.SetActive(_killCount == 2);
            _node_kill3.gameObject.SetActive(_killCount == 3);
            _node_kill4.gameObject.SetActive(_killCount == 4);
            _node_kill5.gameObject.SetActive(_killCount >= 5);
            _killTipShowing = true;
        }

        private void HideKillTip()
        {
            _node_kill1.gameObject.SetActive(false);
            _node_kill2.gameObject.SetActive(false);
            _node_kill3.gameObject.SetActive(false);
            _node_kill4.gameObject.SetActive(false);
            _node_kill5.gameObject.SetActive(false);
            _killTipShowing = false;
        }

        #region Joystick

        private void InitJoystick()
        {
            _moveJoystick = _node_joy_move.GetComponent<JoystickWidget>();
            _attackJoystick = _node_joy_attack.GetComponent<JoystickWidget>();
            _skillJoystick = _node_joy_skill.GetComponent<JoystickWidget>();
            _superJoystick = _node_joy_super.GetComponent<JoystickWidget>();

            _moveJoystick.OnInit();
            _attackJoystick.OnInit();
            _skillJoystick.OnInit();
            _superJoystick.OnInit();
        }

        private void OpenJoystick()
        {
            // Open joystick
            _moveJoystick.OnOpen(null);
            _attackJoystick.OnOpen(null);
            _skillJoystick.OnOpen(null);
            _superJoystick.OnOpen(null);

            _moveJoystick.OnDrag += OnMoveDrag;
            _attackJoystick.OnDrag += OnAttackDrag;
            _skillJoystick.OnDrag += OnSkillDrag;
            _superJoystick.OnDrag += OnSuperDrag;

            _moveJoystick.OnDown += OnStartMove;
            _attackJoystick.OnDown += OnStartAttack;
            _skillJoystick.OnDown += OnStartSkill;
            _superJoystick.OnDown += OnStartSuper;

            _moveJoystick.OnUp += OnEndMove;
            _attackJoystick.OnUp += OnEndAttack;
            _skillJoystick.OnUp += OnEndSkill;
            _superJoystick.OnUp += OnEndSuper;
        }

        private void CloseJoystick()
        {
            _moveJoystick.OnClose();
            _attackJoystick.OnClose();
            _skillJoystick.OnClose();
            _superJoystick.OnClose();
        }

        private void TickJoystick(float dt)
        {
            _moveJoystick.Tick(dt);
            _attackJoystick.Tick(dt);
            _skillJoystick.Tick(dt);
            _superJoystick.Tick(dt);
        }

        // End Joystick
        private void OnEndMove()
        {
            G.Input.Data.moveDir = Vector2.zero;
        }

        private void OnEndAttack()
        {
            G.Input.Data.doAttack = true;
            G.Input.Data.attackPrepare = false;
        }

        private void OnEndSkill()
        {
            G.Input.Data.doSkill = true;
            G.Input.Data.skillPrepare = false;
        }

        private void OnEndSuper()
        {
            G.Input.Data.doSuperSkill = true;
            G.Input.Data.superSkillPrepare = false;
        }

        // Start Joystick
        private void OnStartMove()
        {
        }

        private void OnStartAttack()
        {
            G.Input.Data.attackPrepare = true;
        }

        private void OnStartSkill()
        {
            G.Input.Data.skillPrepare = true;
        }

        private void OnStartSuper()
        {
            G.Input.Data.superSkillPrepare = true;
        }

        // Drag Joystick
        private void OnMoveDrag(Vector2 vec)
        {
            G.Input.Data.moveDir = vec;
            JLog.Debug($"OnMove {vec}");
        }

        private void OnAttackDrag(Vector2 vec)
        {
            G.Input.Data.aimVector = vec;
        }

        private void OnSkillDrag(Vector2 vec)
        {
            G.Input.Data.aimVector = vec;
        }

        private void OnSuperDrag(Vector2 vec)
        {
            G.Input.Data.aimVector = vec;
        }

        #endregion
    }

}