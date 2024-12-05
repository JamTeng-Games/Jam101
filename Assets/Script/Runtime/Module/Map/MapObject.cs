using System;
using Photon.Deterministic;
using Quantum;
using UnityEngine;

namespace Jam.Runtime
{

    [Serializable]
    public struct MapObjectData
    {
        public MapObjectType objectType;
        public FP height;
    }

    [Serializable]
    public enum MapObjectType
    {
        Ground,
        Block,
        Water,
        Shrub,
    }

    // [ExecuteInEditMode]
    public class MapObject : MonoBehaviour
    {
        public const int GridUnit = 2;

        public MapObjectData Data;
#if UNITY_EDITOR

        public int xMin;
        public int xMax;
        public int zMin;
        public int zMax;
        public float yMin;
        public float yMax;

        private Renderer _renderer;

        public void ResetMinMax()
        {
            if (_renderer == null)
                _renderer = GetComponent<Renderer>();
            Bounds bounds = _renderer.bounds;
            xMin = Mathf.RoundToInt(bounds.min.x * 2);
            zMin = Mathf.RoundToInt(bounds.min.z * 2);
            xMax = Mathf.RoundToInt(bounds.max.x * 2);
            zMax = Mathf.RoundToInt(bounds.max.z * 2);

            if (xMin == xMax)
            {
                xMin--;
                xMax++;
            }

            if (zMin == zMax)
            {
                zMin--;
                zMax++;
            }

            yMin = bounds.min.y;
            yMax = bounds.max.y;
            Data.height = FP.FromFloat_UNSAFE(bounds.size.y);
        }

        public void ResetQuantumBox2D()
        {
            if (!gameObject.TryGetComponent<BoxCollider>(out var box))
                box = gameObject.AddComponent<BoxCollider>();
            if (!gameObject.TryGetComponent<QuantumStaticBoxCollider2D>(out var qc))
            {
                qc = gameObject.AddComponent<QuantumStaticBoxCollider2D>();
                qc.Height = Data.height;
                qc.Settings.Trigger = Data.objectType != MapObjectType.Block;
            }
            else
            {
                qc.Settings.Trigger = Data.objectType != MapObjectType.Block;
            }
            qc.SourceCollider = box;
        }

        public void RemoveBox()
        {
            // if (gameObject.TryGetComponent<Collider>(out var collider))
            //     DestroyImmediate(collider);
            // if (gameObject.TryGetComponent<QuantumStaticBoxCollider2D>(out var qc))
            //     qc.SourceCollider = null;
        }

        private void Reset()
        {
            ResetMinMax();
            ResetQuantumBox2D();
        }

        private void OnValidate()
        {
            ResetMinMax();
            // if (gameObject.TryGetComponent<QuantumStaticBoxCollider2D>(out var qc))
            //     qc.Settings.Trigger = Data.objectType != MapObjectType.Block;
            // Debug.Log("OnValidate");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Data.objectType switch
            {
                MapObjectType.Ground => Color.yellow,
                MapObjectType.Block  => Color.red,
                MapObjectType.Water  => Color.blue,
                MapObjectType.Shrub  => Color.green,
            };
            Vector3 center = new Vector3(xMax + xMin, 0, zMax + zMin) / 4f;
            Vector3 size = new Vector3(xMax - xMin, 0.1f, zMax - zMin) / 2f;
            center.y = yMin - 0.1f;
            Gizmos.DrawCube(center, size);
        }

#endif
    }

}