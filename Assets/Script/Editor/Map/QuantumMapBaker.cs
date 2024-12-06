using Jam.Runtime;
using Quantum;
using UnityEngine;

namespace Jam.Editor_
{

    [MapDataBakerCallback(invokeOrder: 5)]
    [assembly: Quantum.QuantumMapBakeAssemblyAttribute]
    public class QuantumMapBaker : MapDataBakerCallback
    {
        public override void OnBeforeBake(QuantumMapData data)
        {
            Debug.Log("OnBeforeBake");
        }

        public override void OnBake(QuantumMapData data)
        {
            Debug.Log("OnBake");
            var customData = QuantumUnityDB.GetGlobalAssetEditorInstance<CustomMapAsset>(data.Asset.UserAsset);
            var mapObjs = Object.FindObjectsByType<MapObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            if (customData == null || mapObjs.Length == 0)
                return;

            foreach (var mapObj in mapObjs)
            {
                for (int x = mapObj.xMin; x <= mapObj.xMax; x++)
                {
                    for (int z = mapObj.zMin; z <= mapObj.zMax; z++)
                    {
                        customData.Set(
                            x, z,
                            new MapObjectData { objectType = mapObj.Data.objectType, height = mapObj.Data.height });
                    }
                }
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(customData);
#endif
        }
    }

}