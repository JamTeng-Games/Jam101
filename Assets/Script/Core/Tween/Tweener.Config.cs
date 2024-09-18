// namespace J.Core
// {
//
//     public class TweenerConfig
//     {
//         // Const
//         private float _delay;
//         private float _duration;
//         private EaseType _easeType = EaseType.Linear; // EaseType.QuadOut
//         private float _timeScale = 1f;
//         private int _repeat;
//         private bool _yoyo;
//         private TweenValue _startValue;
//         private TweenValue _value;
//         private TweenValue _endValue;
//         private TweenValue _deltaValue;
//         private int _valueSize;
//         // Const
//
//         private System.Action<TweenValue> _startCallback;
//         private System.Action<TweenValue> _updateCallback;
//         private System.Action<TweenValue> _endCallback;
//
//         public TweenerConfig SetDelay(float delay)
//         {
//             _delay = delay;
//             _delayTime = 0f;
//             return this;
//         }
//
//         public TweenerConfig SetEase(EaseType easeType)
//         {
//             _easeType = easeType;
//             return this;
//         }
//
//         public TweenerConfig SetRepeat(int times, bool yoyo = false)
//         {
//             _repeat = times;
//             _yoyo = yoyo;
//             return this;
//         }
//
//         public TweenerConfig SetYoyo(bool yoyo)
//         {
//             _yoyo = yoyo;
//             return this;
//         }
//
//         public TweenerConfig SetTimeScale(float scale)
//         {
//             _timeScale = scale;
//             return this;
//         }
//
//         public TweenerConfig OnStart(System.Action<TweenValue> callback)
//         {
//             _startCallback += callback;
//             return this;
//         }
//
//         public TweenerConfig OnUpdate(System.Action<TweenValue> callback)
//         {
//             _updateCallback += callback;
//             return this;
//         }
//
//         public TweenerConfig OnEnd(System.Action<TweenValue> callback)
//         {
//             _endCallback += callback;
//             return this;
//         }
//     }
//
// }