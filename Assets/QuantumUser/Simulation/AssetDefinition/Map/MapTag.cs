using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quantum
{

    public enum EMapTag
    {
        Ground,
        Block,
        Water,
    }

    struct MyStruct
    {
    }

    [ExecuteInEditMode]
    public class MapTag : MonoBehaviour
    {
        // 一米占几个格子
        public const int GridUnit = 2;

        public EMapTag tag;

        public int xMin;
        public int xMax;
        public int zMin;
        public int zMax;

        public Vector2Int size;

        private Transform testParent;

        private void Reset()
        {
            var collider = GetComponent<Collider>();
            if (collider != null)
                DestroyImmediate(collider);
        }

        private void OnValidate()
        {
            testParent = new GameObject("Test").transform;
            testParent.SetParent(transform);

            var render = GetComponent<Renderer>();
            Bounds bounds = render.bounds;
            var center1 = bounds.center * GridUnit;

            xMin = (int)(bounds.min.x * GridUnit);
            zMin = (int)(bounds.min.z * GridUnit);
            xMax = (int)(bounds.max.x * GridUnit);
            zMax = (int)(bounds.max.z * GridUnit);

            size = new Vector2Int(xMax - xMin, zMax - zMin);
            // for (int x = xMin; x <= xMax; x++)
            // {
            //     for (int y = yMin; y <= yMax; y++)
            //     {
            //         DrawAtPoint(x, y);
            //     }
            // }
        }

        private void DrawAtPoint(int x, int y)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = new Vector3(x / 2f, -1, y / 2f);
            go.transform.SetParent(testParent);
            go.transform.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            go.name = $"{x}_{y}";
        }
    }

}