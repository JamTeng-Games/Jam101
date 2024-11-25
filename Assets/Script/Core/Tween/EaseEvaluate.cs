using System;
using UnityEngine;

namespace Jam.Core
{

    public class EaseEvaluate
    {
        private const float Tolerance = 0.0001f;
        private const float Pi = MathF.PI;
        private const float PiOver2 = Pi * 0.5f;
        private const float TwoPi = Pi * 2;

        public static float Evaluate(EaseType easeType,
                                     float time,
                                     float duration,
                                     AnimationCurve curve = null,
                                     float overshootOrAmplitude = 1.70158f,
                                     float period = 0)
        {
            if (duration <= 0)
                return 1;

            switch (easeType)
            {
                case EaseType.Linear:    return time / duration;
                case EaseType.SineIn:    return -(float)Math.Cos(time / duration * PiOver2) + 1;
                case EaseType.SineOut:   return (float)Math.Sin(time / duration * PiOver2);
                case EaseType.SineInOut: return -0.5f * ((float)Math.Cos(Pi * time / duration) - 1);
                case EaseType.QuadIn:    return (time /= duration) * time;
                case EaseType.QuadOut:   return -(time /= duration) * (time - 2);
                case EaseType.QuadInOut:
                    if ((time /= duration * 0.5f) < 1)
                        return 0.5f * time * time;
                    return -0.5f * ((--time) * (time - 2) - 1);
                case EaseType.CubicIn:  return (time /= duration) * time * time;
                case EaseType.CubicOut: return ((time = time / duration - 1) * time * time + 1);
                case EaseType.CubicInOut:
                    if ((time /= duration * 0.5f) < 1)
                        return 0.5f * time * time * time;
                    return 0.5f * ((time -= 2) * time * time + 2);
                case EaseType.QuartIn:  return (time /= duration) * time * time * time;
                case EaseType.QuartOut: return -((time = time / duration - 1) * time * time * time - 1);
                case EaseType.QuartInOut:
                    if ((time /= duration * 0.5f) < 1)
                        return 0.5f * time * time * time * time;
                    return -0.5f * ((time -= 2) * time * time * time - 2);
                case EaseType.QuintIn:  return (time /= duration) * time * time * time * time;
                case EaseType.QuintOut: return ((time = time / duration - 1) * time * time * time * time + 1);
                case EaseType.QuintInOut:
                    if ((time /= duration * 0.5f) < 1)
                        return 0.5f * time * time * time * time * time;
                    return 0.5f * ((time -= 2) * time * time * time * time + 2);
                case EaseType.ExpoIn: return (time == 0) ? 0 : (float)Math.Pow(2, 10 * (time / duration - 1));
                case EaseType.ExpoOut:
                    if (Math.Abs(time - duration) < Tolerance)
                        return 1;
                    return (-(float)Math.Pow(2, -10 * time / duration) + 1);
                case EaseType.ExpoInOut:
                    if (time == 0)
                        return 0;
                    if (Math.Abs(time - duration) < Tolerance)
                        return 1;
                    if ((time /= duration * 0.5f) < 1)
                        return 0.5f * (float)Math.Pow(2, 10 * (time - 1));
                    return 0.5f * (-(float)Math.Pow(2, -10 * --time) + 2);
                case EaseType.CircIn:  return -((float)Math.Sqrt(1 - (time /= duration) * time) - 1);
                case EaseType.CircOut: return (float)Math.Sqrt(1 - (time = time / duration - 1) * time);
                case EaseType.CircInOut:
                    if ((time /= duration * 0.5f) < 1)
                        return -0.5f * ((float)Math.Sqrt(1 - time * time) - 1);
                    return 0.5f * ((float)Math.Sqrt(1 - (time -= 2) * time) + 1);
                case EaseType.ElasticIn:
                    float s0;
                    if (time == 0)
                        return 0;
                    if (Math.Abs((time /= duration) - 1) < Tolerance)
                        return 1;
                    if (period == 0)
                        period = duration * 0.3f;
                    if (overshootOrAmplitude < 1)
                    {
                        overshootOrAmplitude = 1;
                        s0 = period / 4;
                    }
                    else
                        s0 = period / TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                    return -(overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s0) * TwoPi / period));
                case EaseType.ElasticOut:
                    float s1;
                    if (time == 0)
                        return 0;
                    if (Math.Abs((time /= duration) - 1) < Tolerance)
                        return 1;
                    if (period == 0)
                        period = duration * 0.3f;
                    if (overshootOrAmplitude < 1)
                    {
                        overshootOrAmplitude = 1;
                        s1 = period / 4;
                    }
                    else
                        s1 = period / TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                    return (overshootOrAmplitude * (float)Math.Pow(2, -10 * time) * (float)Math.Sin((time * duration - s1) * TwoPi / period) + 1);
                case EaseType.ElasticInOut:
                    float s;
                    if (time == 0)
                        return 0;
                    if (Math.Abs((time /= duration * 0.5f) - 2) < Tolerance)
                        return 1;
                    if (period == 0)
                        period = duration * (0.3f * 1.5f);
                    if (overshootOrAmplitude < 1)
                    {
                        overshootOrAmplitude = 1;
                        s = period / 4;
                    }
                    else
                        s = period / TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                    if (time < 1)
                        return -0.5f *
                               (overshootOrAmplitude *
                                (float)Math.Pow(2, 10 * (time -= 1)) *
                                (float)Math.Sin((time * duration - s) * TwoPi / period));
                    return overshootOrAmplitude *
                           (float)Math.Pow(2, -10 * (time -= 1)) *
                           (float)Math.Sin((time * duration - s) * TwoPi / period) *
                           0.5f +
                           1;
                case EaseType.BackIn:  return (time /= duration) * time * ((overshootOrAmplitude + 1) * time - overshootOrAmplitude);
                case EaseType.BackOut: return ((time = time / duration - 1) * time * ((overshootOrAmplitude + 1) * time + overshootOrAmplitude) + 1);
                case EaseType.BackInOut:
                    if ((time /= duration * 0.5f) < 1)
                        return 0.5f * (time * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time - overshootOrAmplitude));
                    return 0.5f * ((time -= 2) * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time + overshootOrAmplitude) + 2);
                case EaseType.BounceIn:    return Bounce.EaseIn(time, duration);
                case EaseType.BounceOut:   return Bounce.EaseOut(time, duration);
                case EaseType.BounceInOut: return Bounce.EaseInOut(time, duration);
                case EaseType.Custom:      return curve.Evaluate(time / duration);

                default: return -(time /= duration) * (time - 2);
            }
        }

        private static class Bounce
        {
            public static float EaseIn(float time, float duration)
            {
                return 1 - EaseOut(duration - time, duration);
            }

            public static float EaseOut(float time, float duration)
            {
                if ((time /= duration) < (1 / 2.75f))
                {
                    return (7.5625f * time * time);
                }
                if (time < (2 / 2.75f))
                {
                    return (7.5625f * (time -= (1.5f / 2.75f)) * time + 0.75f);
                }
                if (time < (2.5f / 2.75f))
                {
                    return (7.5625f * (time -= (2.25f / 2.75f)) * time + 0.9375f);
                }
                return (7.5625f * (time -= (2.625f / 2.75f)) * time + 0.984375f);
            }

            public static float EaseInOut(float time, float duration)
            {
                if (time < duration * 0.5f)
                {
                    return EaseIn(time * 2, duration) * 0.5f;
                }
                return EaseOut(time * 2 - duration, duration) * 0.5f + 0.5f;
            }
        }
    }

}