using System.Reflection;
using Photon.Deterministic;
using UnityEngine;
using Plane = UnityEngine.Plane;

namespace Quantum.Helper
{

    public static unsafe class Helper_Math
    {
        public static bool IsPointInSector(FPVector2 point, FPVector2 center, FP radius, FP startAngleRad, FP endAngleRad)
        {
            if (startAngleRad < 0)
                startAngleRad += FP.Pi * 2;
            if (endAngleRad < 0)
                endAngleRad += FP.Pi * 2;

            // 计算向量
            FPVector2 vector = point - center;

            // 计算角度（使用Atan2来处理所有象限的角度）
            FP angle = FPMath.Atan2(vector.Y, vector.X);
            if (angle < 0)
                angle += FP.Pi * 2;

            // // 检查点是否在圆内
            if (vector.Magnitude > radius)
                return false;

            // 检查角度是否在扇形内
            if (startAngleRad <= endAngleRad)
            {
                return angle >= startAngleRad && angle <= endAngleRad;
            }
            else
            {
                return angle >= startAngleRad || angle <= endAngleRad;
            }
        }

        public static FP DirectionToAngleRad(FPVector2 direction)
        {
            return FPMath.Atan2(direction.Y, direction.X);
        }

        public static FPVector2 AngleRadToDirection(FP angle)
        {
            FP x = FPMath.Cos(angle);
            FP y = FPMath.Sin(angle);
            return new FPVector2(x, y);
        }

        public static Vector2 AngleRadToDirectionF(float angle)
        {
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            return new Vector2(x, y);
        }

        public static FPVector2 RandomPosition(Frame f, FP radius)
        {
            FP x = f.RNG->Next(-radius, radius);
            FP y = f.RNG->Next(-radius, radius);
            return new FPVector2(x, y);
        }
    }

}