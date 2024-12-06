using System;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quantum
{

    [Serializable]
    public enum MapObjectType
    {
        None,
        Ground,
        Block,
        Water,
        Shrub,
    }

    [Serializable]
    public class MapObjectData
    {
        public MapObjectType objectType;
        public FP height;
    }

    public class CustomMapAsset : AssetObject
    {
        public int MapX;
        public int MapZ;
        public List<MapObjectData> Grid;

        public (MapObjectType type, FP height) GetPosInfo(FPVector2 pos)
        {
            var (x, z) = WorldPosToGrid(pos);
            if (TryGet(x, z, out var data))
            {
                return (data.objectType, data.height);
            }
            return (MapObjectType.None, FP._0);
        }

        public bool TryGet(int x, int z, out MapObjectData data)
        {
            data = default;
            if (Grid == null)
                return false;
            int index = GetIndex(x, z);
            if (index < 0 || index >= Grid.Count)
                return false;
            if (Grid[index] == null)
                return false;
            data = Grid[index];
            return true;
        }

        public (int x, int z) GetPosByIndex(int index)
        {
            int realX = index % MapX;
            int realZ = index / MapZ;
            int x = realX - MapX / 2;
            int z = realZ - MapZ / 2;
            return (x, z);
        }

        public void Set(int x, int z, MapObjectData data)
        {
            if (Grid == null)
                Grid = new List<MapObjectData>(new MapObjectData[MapX * MapZ]);
            int index = GetIndex(x, z);
            if (index < 0)
                return;
            Grid[index] = data;
        }

        public void SetMapSize(int x, int z)
        {
            MapX = x * 2;
            MapZ = z * 2;
        }

        public void ClearAll()
        {
            if (Grid != null)
                Grid.Clear();
            Grid = null;
            MapX = 0;
            MapZ = 0;
        }

        public int GetIndex(int x, int z)
        {
            int realX = x + MapX / 2;
            int realZ = z + MapZ / 2;
            return realX + MapZ * realZ;
        }

        private (int x, int z) WorldPosToGrid(FPVector2 pos)
        {
            var p = pos * 2;
            return (FPMath.FloorToInt(p.X), FPMath.FloorToInt(p.Y));
        }
    }

}