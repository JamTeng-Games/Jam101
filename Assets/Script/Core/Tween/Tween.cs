using System.Collections.Generic;
using UnityEngine;

namespace Jam.Core
{

    // A sample can be found in Test/TestTween
    public static class Tween
    {
        private static List<Tweener> _runningList;
        private static List<Tweener> _waitingList;

        static Tween()
        {
            _runningList = new List<Tweener>();
            _waitingList = new List<Tweener>();
        }

        public static Tweener CreateAndPlay(float start, float end, float duration)
        {
            var tweener = Tweener.Create(start, end, duration);
            Play(tweener);
            return tweener;
        }

        public static Tweener CreateAndPlay(Vector2 start, Vector2 end, float duration)
        {
            var tweener = Tweener.Create(start, end, duration);
            Play(tweener);
            return tweener;
        }

        public static Tweener CreateAndPlay(Vector3 start, Vector3 end, float duration)
        {
            var tweener = Tweener.Create(start, end, duration);
            Play(tweener);
            return tweener;
        }

        public static Tweener CreateAndPlay(Vector4 start, Vector4 end, float duration)
        {
            var tweener = Tweener.Create(start, end, duration);
            Play(tweener);
            return tweener;
        }

        public static Tweener CreateAndPlay(Quaternion start, Quaternion end, float duration)
        {
            var tweener = Tweener.Create(start, end, duration);
            Play(tweener);
            return tweener;
        }

        public static Tweener CreateAndPlay(Color start, Color end, float duration)
        {
            var tweener = Tweener.Create(start, end, duration);
            Play(tweener);
            return tweener;
        }

        public static Tweener CreateAndPlay(TweenValue start, TweenValue end, float duration)
        {
            var tweener = Tweener.Create(start, end, duration);
            Play(tweener);
            return tweener;
        }

        public static void Play(Tweener tweener)
        {
            _waitingList.Add(tweener);
        }

        public static void Stop(Tweener tweener)
        {
            tweener.Stop();
        }

        public static void Pause(Tweener tweener)
        {
            tweener.Pause();
        }

        // public void Tick(float deltaTime)
        public static void Tick(float deltaTime)
        {
            for (int i = _runningList.Count - 1; i >= 0; i--)
            {
                Tweener tweener = _runningList[i];
                if (tweener.IsPaused)
                    continue;

                tweener.Update(deltaTime);
                if (!tweener.IsPlaying)
                {
                    _runningList.RemoveAt(i);
                    // tweener.Dispose();
                }
            }

            for (int i = _waitingList.Count - 1; i >= 0; i--)
            {
                Tweener tweener = _waitingList[i];
                if (tweener.IsStop)
                {
                    _waitingList.RemoveAt(i);
                    continue;
                }

                tweener.UpdateDelayTime(deltaTime);
                if (tweener.IsReady)
                {
                    _waitingList.RemoveAt(i);
                    _runningList.Add(tweener);
                    tweener.Play();
                    continue;
                }
                if (!tweener.IsWaiting)
                {
                    _waitingList.RemoveAt(i);
                }
            }
        }
    }

}