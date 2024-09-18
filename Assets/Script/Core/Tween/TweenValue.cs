using System;
using UnityEngine;

namespace J.Core
{

    public class TweenValue
    {
        public float x;
        public float y;
        public float z;
        public float w;
        public double d;

        public bool IsFloat { get; private set; }
        public bool IsVec2 { get; private set; }
        public bool IsVec3 { get; private set; }
        public bool IsVec4 { get; private set; }
        public bool IsColor { get; private set; }
        public bool IsQuat { get; private set; }

        public static TweenValue Create(TweenValue clone)
        {
            TweenValue tweenValue = new TweenValue();
            tweenValue.Clone(clone);
            return tweenValue;
        }

        public float Value
        {
            get => x;
            set
            {
                IsFloat = true;
                x = value;
            }
        }

        public Vector2 Vec2
        {
            get => new Vector2(x, y);
            set
            {
                IsVec2 = true;
                x = value.x;
                y = value.y;
            }
        }

        public Vector3 Vec3
        {
            get => new Vector3(x, y, z);
            set
            {
                IsVec3 = true;
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }

        public Vector4 Vec4
        {
            get => new Vector4(x, y, z, w);
            set
            {
                IsVec4 = true;
                x = value.x;
                y = value.y;
                z = value.z;
                w = value.w;
            }
        }

        public Quaternion Quat
        {
            get => new Quaternion(x, y, z, w);
            set
            {
                IsQuat = true;
                x = value.x;
                y = value.y;
                z = value.z;
                w = value.w;
            }
        }

        public Color Color
        {
            get => new Color(x, y, z, w);
            set
            {
                IsColor = true;
                x = value.r;
                y = value.g;
                z = value.b;
                w = value.a;
            }
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:  return x;
                    case 1:  return y;
                    case 2:  return z;
                    case 3:  return w;
                    default: throw new Exception("Index out of bounds: " + index);
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    case 3:
                        w = value;
                        break;
                    default: throw new Exception("Index out of bounds: " + index);
                }
            }
        }

        public void SetZero()
        {
            x = y = z = w = 0;
            d = 0;
        }

        public void Clone(TweenValue clone)
        {
            x = clone.x;
            y = clone.y;
            z = clone.z;
            w = clone.w;
            d = clone.d;
        }
    }

}