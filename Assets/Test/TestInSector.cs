using System;
using Jam.Core;
using UnityEngine;

namespace Jam.Test
{

    public class TestInSector : MonoBehaviour
    {
        public Transform checkPoint;

        private void Update()
        {
            Vector2 point = new Vector2(checkPoint.position.x, checkPoint.position.z);

            float _60 = Mathf.PI / 3f;
            float _90 = Mathf.PI / 2f;
            float startRad = -_60 + _90;
            float endRad = _60 + _90;

            bool isIn = IsPointInSector(point, new Vector2(transform.position.x, transform.position.z), 2, startRad, endRad);
            Debug.Log($"Is in {isIn}");
        }

        public static bool IsPointInSector(Vector2 point, Vector2 center, float radius, float startAngleRad, float endAngleRad)
        {
            // 计算向量
            Vector2 vector = point - center;

            DebugExtension.DebugArrow(Vector3.zero, new Vector3(vector.x, 0, vector.y));

            // 计算角度（使用Atan2来处理所有象限的角度）
            float angle = Mathf.Atan2(vector.y, vector.x);
            if (angle < 0)
                angle += Mathf.PI * 2; // 转换到 [0, 2π) 范围

            // 检查点是否在圆内
            if (vector.magnitude > radius)
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
    }

}