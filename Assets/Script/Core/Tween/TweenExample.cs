namespace Jam.Core
{

    public class TweenExample
    {
        public static void Example()
        {
            float from = -10f;
            float to = -10f;
            float duration = 2f;

            Tween.CreateAndPlay(from, to, duration)
                 .SetDelay(1f)
                 .SetEase(EaseType.BounceOut)
                 .SetRepeat(5)
                 .SetYoyo(true)
                 .SetTimeScale(0.6f)
                 .OnUpdate(value =>
                 {
                     JLog.Info(value.Value);
                 });
        }
    }

}