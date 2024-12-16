using Animancer;
using Quantum;
using Quantum.Graph.Skill;
using UnityEngine;

namespace Jam.Arena
{

    [Quantum.Core.RequireComponent(typeof(AnimancerComponent))]
    public class AnimateComp : JamEntityViewComp
    {
        private AnimancerComponent _animancer;
        private AnimationInfo _currentAnim;
        private int _startPlayingFrame;
        private float _currentAnimRunningTime;

        public override void OnInitialize()
        {
            _animancer = this.GetComponent<AnimancerComponent>();
        }

        public override void OnActivate(Frame frame)
        {
            QuantumEvent.Subscribe<EventOnMove>(listener: this, handler: OnMoveEvent);
            QuantumEvent.Subscribe<EventOnStopMove>(listener: this, handler: OnStopMoveEvent);
            QuantumEvent.Subscribe<EventPlayAnim>(listener: this, handler: OnPlayAnimEvent);
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
            Play(AnimationKey.Run);
        }

        private void OnStopMoveEvent(EventOnStopMove callback)
        {
            if (callback.entity != EntityRef)
                return;
            if (_currentAnim.key == AnimationKey.Run)
                Play(AnimationKey.Idle, true);
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
            var animInfo = ViewContext.animationBank.GetAnimation(animKey);
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
                var state = _animancer.Play(animInfo.animation);
                state.Time = 0;
                state.Speed = 1.2f;
            }
        }
    }

}