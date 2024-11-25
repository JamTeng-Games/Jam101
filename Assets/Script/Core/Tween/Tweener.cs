using System;
using UnityEngine;

namespace Jam.Core
{

    public partial class Tweener
    {
        private enum TweenState
        {
            Init,
            Stop,
            Waiting,
            Ready,
            Playing,
            Pause,
        }

        private static uint s_id = 1;

        private uint _id;
        private TweenState _state = TweenState.Init;
        private float _time;
        private float _delayTime;
        private float _normalizedTime;
        private int _repeatTime;
        private TweenValue _startRunningValue;
        private TweenValue _endRunningValue;

        // Const
        private float _delay;
        private float _duration;
        private EaseType _easeType = EaseType.Linear; // EaseType.QuadOut
        private float _timeScale = 1f;
        private int _repeat;
        private bool _yoyo;
        private TweenValue _startValue = new TweenValue();
        public TweenValue _value = new TweenValue();
        private TweenValue _endValue = new TweenValue();
        private TweenValue _deltaValue = new TweenValue();
        private int _valueSize;
        private AnimationCurve _curve;
        // Const

        private Action<TweenValue> _startCallback;
        private Action<TweenValue> _updateCallback;
        private Action<TweenValue> _endCallback;

        // async Task support
        // private UniTaskCompletionSource<Tweener> _taskSource;

        // Public
        public uint Id => _id;
        public float Time => _time;
        public float Delay => _delay;
        public float Duration => _duration;
        public float TimeScale => _timeScale;
        public int Repeat => _repeat;
        public bool Yoyo => _yoyo;
        public TweenValue StartValue => _startValue;
        public TweenValue Value => _value;
        public TweenValue EndValue => _endValue;
        public TweenValue DeltaValue => _deltaValue;

        public bool IsWaiting => _state == TweenState.Waiting;
        public bool IsReady => _state == TweenState.Ready;
        public bool IsPlaying => _state == TweenState.Playing;
        public bool IsPaused => _state == TweenState.Pause;
        public bool IsStop => _state == TweenState.Stop;

        public static Tweener Create(float start, float end, float duration)
        {
            Tweener tween = new Tweener();
            tween._id = s_id++;
            tween._valueSize = 1;
            tween._startValue.Value = start;
            tween._endValue.Value = end;
            tween._duration = duration;
            // Task
            // tween._taskSource = new UniTaskCompletionSource<Tweener>();
            return tween;
        }

        public static Tweener Create(Vector2 start, Vector2 end, float duration)
        {
            Tweener tween = new Tweener();
            tween._id = s_id++;
            tween._valueSize = 2;
            tween._startValue.Vec2 = start;
            tween._endValue.Vec2 = end;
            tween._duration = duration;
            // Task
            // tween._taskSource = new UniTaskCompletionSource<Tweener>();
            return tween;
        }

        public static Tweener Create(Vector3 start, Vector3 end, float duration)
        {
            Tweener tween = new Tweener();
            tween._id = s_id++;
            tween._valueSize = 3;
            tween._startValue.Vec3 = start;
            tween._endValue.Vec3 = end;
            tween._duration = duration;
            // Task
            // tween._taskSource = new UniTaskCompletionSource<Tweener>();
            return tween;
        }

        public static Tweener Create(Vector4 start, Vector4 end, float duration)
        {
            Tweener tween = new Tweener();
            tween._id = s_id++;
            tween._valueSize = 4;
            tween._startValue.Vec4 = start;
            tween._endValue.Vec4 = end;
            tween._duration = duration;
            // Task
            // tween._taskSource = new UniTaskCompletionSource<Tweener>();
            return tween;
        }

        public static Tweener Create(Quaternion start, Quaternion end, float duration)
        {
            Tweener tween = new Tweener();
            tween._id = s_id++;
            tween._valueSize = 4;
            tween._startValue.Quat = start;
            tween._endValue.Quat = end;
            tween._duration = duration;
            // Task
            // tween._taskSource = new UniTaskCompletionSource<Tweener>();
            return tween;
        }

        public static Tweener Create(Color start, Color end, float duration)
        {
            Tweener tween = new Tweener();
            tween._id = s_id++;
            tween._valueSize = 4;
            tween._startValue.Color = start;
            tween._endValue.Color = end;
            tween._duration = duration;
            // Task
            // tween._taskSource = new UniTaskCompletionSource<Tweener>();
            return tween;
        }

        public static Tweener Create(TweenValue start, TweenValue end, float duration)
        {
            Tweener tween = new Tweener();
            tween._id = s_id++;
            tween._valueSize = 4;
            tween._startValue = start;
            tween._endValue = end;
            // tween._value = start;
            tween._duration = duration;
            // Task
            // tween._taskSource = new UniTaskCompletionSource<Tweener>();
            return tween;
        }

        /// Don't call it directly!
        public void Dispose()
        {
            // ReferencePool.Release(this);
        }

        public void Play()
        {
            _state = TweenState.Playing;
            _time = 0f;
            _startRunningValue = _startValue;
            _endRunningValue = _endValue;
            try
            {
                _startCallback?.Invoke(_startValue);
            }
            catch (Exception e)
            {
                JLog.Exception(e);
            }
        }

        public void Stop()
        {
            if (_state == TweenState.Stop)
                return;
            // _taskSource.TrySetResult(this);
            _state = TweenState.Stop;
        }

        public void Pause()
        {
            _state = TweenState.Pause;
        }

        public void Resume()
        {
            _state = TweenState.Playing;
        }

        // }

        public void Update(float deltaTime)
        {
            if (_state != TweenState.Playing)
                return;

            if (_timeScale == 0)
                Stop();

            deltaTime *= _timeScale;
            _time += deltaTime;

            if (_yoyo)
            {
                bool reversed = _repeatTime % 2 != 0;
                _startRunningValue = reversed ? _endValue : _startValue;
                _endRunningValue = reversed ? _startValue : _endValue;
            }

            _normalizedTime = EaseEvaluate.Evaluate(_easeType, _time, _duration, _curve);

            if (_startValue.IsQuat)
            {
                _value.Quat = Quaternion.Slerp(_startValue.Quat, _endValue.Quat, _normalizedTime);
            }
            else
            {
                for (int i = 0; i < _valueSize; i++)
                {
                    float n1 = _startRunningValue[i];
                    float n2 = _endRunningValue[i];
                    float f = n1 + (n2 - n1) * _normalizedTime;
                    _deltaValue[i] = f - _value[i];
                    _value[i] = f;
                }
            }

            if (_time <= _duration)
            {
                try
                {
                    _updateCallback?.Invoke(_value);
                }
                catch (Exception e)
                {
                    JLog.Exception(e);
                }
            }
            else
            {
                // Repeat
                if (_repeatTime < _repeat)
                {
                    _repeatTime++;
                    _time = 0f;
                }
                else
                {
                    Stop();
                    try
                    {
                        _value.Clone(_endRunningValue);
                        _updateCallback?.Invoke(_endRunningValue);
                        _endCallback?.Invoke(_endRunningValue);
                    }
                    catch (Exception e)
                    {
                        JLog.Exception(e);
                    }
                }
            }
        }

        public void UpdateDelayTime(float deltaTime)
        {
            if (_state == TweenState.Init)
                _state = TweenState.Waiting;
            if (_state != TweenState.Waiting)
                return;

            _delayTime += deltaTime;
            if (_delayTime > _delay)
                _state = TweenState.Ready;
        }

        // public UniTask<Tweener> ToUniTask()
        // {
        //     return _taskSource.Task;
        // }

        public void CancelTask()
        {
            // _taskSource.TrySetCanceled();
            Stop();
        }

        #region Setting

        public Tweener SetDelay(float delay)
        {
            _delay = delay;
            _delayTime = 0f;
            return this;
        }

        public Tweener SetEase(EaseType easeType, AnimationCurve curve = null)
        {
            _easeType = easeType;
            _curve = curve;
            if (easeType == EaseType.Custom && curve == null)
            {
                JLog.Warning("EaseType.Custom need AnimationCurve!");
                _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }
            return this;
        }

        public Tweener SetCurve(AnimationCurve curve)
        {
            return SetEase(EaseType.Custom, curve);
        }

        public Tweener SetRepeat(int times, bool yoyo = false)
        {
            _repeat = times;
            _yoyo = yoyo;
            return this;
        }

        public Tweener SetYoyo(bool yoyo)
        {
            _yoyo = yoyo;
            return this;
        }

        public Tweener SetTimeScale(float scale)
        {
            _timeScale = scale;
            return this;
        }

        public Tweener OnStart(Action<TweenValue> callback)
        {
            _startCallback += callback;
            return this;
        }

        public Tweener OnUpdate(Action<TweenValue> callback)
        {
            _updateCallback += callback;
            return this;
        }

        public Tweener OnEnd(Action<TweenValue> callback)
        {
            _endCallback += callback;
            return this;
        }

        #endregion Setting

        public void Reset()
        {
            _id = 0;
            _state = TweenState.Stop;
            _time = 0f;
            _delayTime = 0f;
            _normalizedTime = 0f;
            _repeatTime = 0;
            _delay = 0f;
            _duration = 0f;
            _easeType = EaseType.QuadOut;
            _timeScale = 1f;
            _repeat = 0;
            _yoyo = false;
            _startValue.SetZero();
            _value.SetZero();
            _endValue.SetZero();
            _deltaValue.SetZero();
            _valueSize = 0;
            _startCallback = null;
            _updateCallback = null;
            _endCallback = null;
        }

        public void Clear()
        {
            Reset();
        }
    }

}