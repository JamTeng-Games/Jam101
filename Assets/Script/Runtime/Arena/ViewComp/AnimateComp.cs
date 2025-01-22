using System;
using System.Collections.Generic;
using Animancer;
using Photon.Deterministic;
using Quantum;
using Quantum.Graph.Skill;
using UnityEngine;

namespace Jam.Arena
{

    [Quantum.Core.RequireComponent(typeof(AnimancerComponent))]
    public class AnimateComp : JamEntityViewComp
    {
        [SerializeField] private AnimationBank _animationBank;
        private AnimancerComponent _animancer;
        private AnimationInfo _currentAnim;
        private AnimancerState _animancerState;
        private int _startPlayingFrame;
        private float _currentAnimRunningTime;
        private bool _isAnimationBankLoaded;
        private bool _isDead;

        // 由于加载animationBank的异步的，在这个期间如果有播动画的请求，会被放到这个队列中
        private Queue<AnimationKey> _waitingQueue = new Queue<AnimationKey>();

        public override void OnInitialize()
        {
            _animancer = this.GetComponent<AnimancerComponent>();
        }

        public override void OnActivate(Frame frame)
        {
            QuantumEvent.UnsubscribeListener(this);

            QuantumEvent.Subscribe<EventOnMove>(listener: this, handler: OnMoveEvent);
            QuantumEvent.Subscribe<EventOnStopMove>(listener: this, handler: OnStopMoveEvent);
            QuantumEvent.Subscribe<EventPlayAnim>(listener: this, handler: OnPlayAnimEvent);
            QuantumEvent.Subscribe<EventOnChangeHp>(listener: this, handler: OnChangeHp);
            // QuantumEvent.Subscribe<EventOnHit>(listener: this, handler: OnHit);
            QuantumEvent.Subscribe<EventOnDie>(listener: this, handler: OnDie);
            QuantumEvent.Subscribe<EventOnReborn>(listener: this, handler: OnReborn);
        }

        private void OnPlayAnimEvent(EventPlayAnim callback)
        {
            if (callback.entity != EntityRef)
                return;
            Play((AnimationKey)callback.animKey, callback.force);
        }

        private void OnMoveEvent(EventOnMove callback)
        {
            if (callback.entity != EntityRef)
                return;
            Log.Debug("OnMove");

            if (callback.velocity.Magnitude < FP._0_10)
            {
                if (_currentAnim.key == AnimationKey.Run)
                    Play(AnimationKey.Idle, true);
            }
            else
            {
                Play(AnimationKey.Run);
            }
        }

        private void OnStopMoveEvent(EventOnStopMove callback)
        {
            if (callback.entity != EntityRef)
                return;
            if (_currentAnim.key == AnimationKey.Run)
                Play(AnimationKey.Idle, true);
        }

        private void OnDie(EventOnDie callback)
        {
            if (callback.entity != EntityRef)
                return;
            _isDead = true;
            Play(AnimationKey.Die);
        }

        private void OnReborn(EventOnReborn callback)
        {
            _isDead = false;
            Play(AnimationKey.Idle);
        }

        private void OnHit(EventOnHit callback)
        {
            if (callback.entity != EntityRef)
                return;
            Play(AnimationKey.Hit);
        }

        private void OnChangeHp(EventOnChangeHp callback)
        {
            if (callback.entity != EntityRef)
                return;
            Log.Debug($"Change Hp {callback.modHp}");
        }

        public override void OnDeactivate()
        {
            QuantumEvent.UnsubscribeListener(this);
        }

        public override void OnUpdateView()
        {
            if (_currentAnim.IsValid())
            {
                _currentAnimRunningTime += Time.deltaTime;
                if (_currentAnimRunningTime > _currentAnim.animation.Length)
                {
                    _currentAnimRunningTime = 0;
                    _currentAnim = AnimationInfo.InValid;
                }
            }
            else
            {
                Play(AnimationKey.Idle);
            }
        }

        public void Play(AnimationKey animKey, bool forceInterrupt = false)
        {
            if (_isDead && animKey != AnimationKey.Die)
                return;

            if (_animationBank == null)
            {
                if (forceInterrupt)
                    _waitingQueue.Clear();
                _waitingQueue.Enqueue(animKey);
                return;
            }

            var animInfo = _animationBank.GetAnimation(animKey);
            if (!animInfo.IsValid() || animInfo.animation == null)
                return;

            // 检查是否需要播放新动画
            if (!_currentAnim.IsValid() ||                                                    // 当前没有正在播放的动画
                forceInterrupt ||                                                             // 是强制播放
                (animInfo.priority > _currentAnim.priority && animKey != _currentAnim.key) || // 优先级更高
                (animKey == _currentAnim.key && _currentAnim.canReplay))                      // 相同动画，可以重播
            {
                _currentAnim = animInfo;
                _currentAnimRunningTime = 0;
                _animancerState = _animancer.Play(animInfo.animation);
                _animancerState.Time = 0;
                _animancerState.Speed = 1f;
            }
        }
    }

}