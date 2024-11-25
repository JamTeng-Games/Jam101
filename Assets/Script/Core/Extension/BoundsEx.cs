using System;
using UnityEngine;

namespace Jam.Core
{

    public static class BoundsEx
    {
        public static Bounds Rotate(this Bounds bounds, Quaternion rotation)
        {
            Span<Vector3> points = stackalloc Vector3[8];
            Vector3 ext = bounds.extents;
            points[0] = new Vector3(ext.x, ext.y, ext.z);
            points[1] = new Vector3(-ext.x, ext.y, ext.z);
            points[2] = new Vector3(ext.x, -ext.y, ext.z);
            points[3] = new Vector3(ext.x, ext.y, -ext.z);
            points[4] = new Vector3(-ext.x, -ext.y, -ext.z);
            points[5] = new Vector3(ext.x, -ext.y, -ext.z);
            points[6] = new Vector3(-ext.x, ext.y, -ext.z);
            points[7] = new Vector3(-ext.x, -ext.y, ext.z);

            Bounds rotatedBounds = new Bounds(bounds.center, Vector3.zero);
            foreach (Vector3 point in points)
            {
                Vector3 rotatedPoint = rotation * point;
                rotatedBounds.Encapsulate(rotatedPoint + bounds.center);
            }
            return rotatedBounds;
        }

        public static Bounds Inflate(this Bounds bounds, float amount)
        {
            Bounds newB = bounds;
            newB.size += amount * Vector3.one;
            return newB;
        }
    }

}