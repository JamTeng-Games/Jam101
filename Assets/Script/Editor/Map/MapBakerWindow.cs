using Jam.Runtime;
using Quantum;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Jam.Editor_
{

    public class MapBakerWindow : EditorWindow
    {
        private static Map _mapAsset;
        private static Map _lastMapAsset;
        private static bool _showTest;
        private Transform _mapBakerTester;
        private CustomMapAsset _customData;

        [MenuItem("Tools/地图烘焙")]
        public static void OpenWindow()
        {
            MapBakerWindow window = GetWindow<MapBakerWindow>();
            window.titleContent = new GUIContent("MapBaker");
            window.Show();
        }

        private void OnEnable()
        {
            GetMapAssetFromCurrentScene();
            EditorApplication.update += Tick;
        }

        private void OnDisable()
        {
            _showTest = false;
            EditorApplication.update -= Tick;
        }

        private void OnGUI()
        {
            // 创建一个可拖放的对象框
            _mapAsset = (Map)EditorGUILayout.ObjectField("地图资产", _mapAsset, typeof(Map), false);
            if (_mapAsset != _lastMapAsset)
            {
                _lastMapAsset = _mapAsset;
                _customData = null;
            }

            EditorGUILayout.Space();

            // 检查是否有拖入的 Map Asset
            if (_mapAsset == null)
            {
                EditorGUILayout.HelpBox("请先设置地图资产.", MessageType.Warning);
            }

            // NOTE: 下面用几个if来判断是为了解决 ArgumentException: Getting control 2's position in a group with only 2 controls when doing repaint
            // 如果提前return或者用else就会报错
            // 不知道什么原理，但是这样就不会报错了
            // 应该是Unity的bug吧我猜

            if (_mapAsset != null && _customData == null)
            {
                _customData = QuantumUnityDB.GetGlobalAssetEditorInstance<CustomMapAsset>(_mapAsset.UserAsset);
                if (_customData == null)
                {
                    EditorGUILayout.HelpBox("请先设置地图自定义数据,Map资产中的UserAsset,需要类型为 CustomMapAsset",
                                            MessageType.Warning);
                }
            }

            if (_mapAsset != null && _customData != null)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("自定义地图数据", _customData, typeof(CustomMapAsset), false);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.Space();

                if (GUILayout.Button("清理"))
                {
                    ClearMap();
                }

                if (GUILayout.Button("烘焙"))
                {
                    BakerMap();
                }

                EditorGUILayout.HelpBox("在场景中添加一个GameObject并设置Tag为MapBakerTester.", MessageType.Info);
                if (GUILayout.Button("测试"))
                {
                    BeginTest();
                }

                if (GUILayout.Button("关闭测试"))
                {
                    EndTest();
                }
            }
        }

        private void Tick()
        {
            if (_showTest && _customData != null && _mapBakerTester != null)
            {
                var info = _customData.GetPosInfo(_mapBakerTester.position.ToFPVector2());
                Debug.Log($"Tester: {info}");
            }
        }

        private void ClearMap()
        {
            Debug.Log("清理场景地图");
            var customData = QuantumUnityDB.GetGlobalAssetEditorInstance<CustomMapAsset>(_mapAsset.UserAsset);
            customData?.ClearAll();
        }

        private void BakerMap()
        {
            Debug.Log("BakerMap");
            var customData = QuantumUnityDB.GetGlobalAssetEditorInstance<CustomMapAsset>(_mapAsset.UserAsset);
            var mapObjs = FindObjectsByType<MapObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            if (customData == null || mapObjs.Length == 0)
                return;

            customData.ClearAll();
            customData.SetMapSize(_mapAsset.WorldSizeX, _mapAsset.WorldSizeY);
            foreach (var mapObj in mapObjs)
            {
                for (int x = mapObj.xMin; x < mapObj.xMax; x++)
                {
                    for (int z = mapObj.zMin; z < mapObj.zMax; z++)
                    {
                        customData.Set(
                            x, z,
                            new MapObjectData { objectType = mapObj.Data.objectType, height = mapObj.Data.height });
                        // Debug.Log($"Set x: {x}, z: {z}, objectType: {mapObj.Data.objectType}, height: {mapObj.Data.height}");
                    }
                }
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(customData);
#endif
            Debug.Log("BakerMap Success");
        }

        private void BeginTest()
        {
            _showTest = false;

            if (_mapBakerTester == null)
            {
                GameObject g = GameObject.FindGameObjectWithTag("MapBakerTester");
                if (g != null)
                    _mapBakerTester = g.transform;
            }

            if (_mapAsset == null)
            {
                Debug.LogError("Map Asset is null");
                return;
            }
            _customData = QuantumUnityDB.GetGlobalAssetEditorInstance<CustomMapAsset>(_mapAsset.UserAsset);
            if (_customData == null)
            {
                Debug.LogError("CustomMapAsset is null");
                return;
            }

            _showTest = true;
            SceneView.RepaintAll();
        }

        private void EndTest()
        {
            _showTest = false;
            _mapBakerTester = null;
        }

        private void GetMapAssetFromCurrentScene()
        {
            _mapAsset = null;
            var quantumMapData = FindObjectOfType<QuantumMapData>();
            if (quantumMapData == null)
                return;
            _mapAsset = quantumMapData.Asset;
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        static void DrawMapGizmos(QuantumMapData mapAssetHolder, GizmoType gizmoType)
        {
            if (!_showTest)
                return;

            var customData = QuantumUnityDB.GetGlobalAssetEditorInstance<CustomMapAsset>(_mapAsset.UserAsset);
            // 遍历 Grid 数组中的每个元素
            for (int i = 0; i < customData.Grid.Count; i++)
            {
                if (customData.Grid[i] == null || customData.Grid[i].objectType == MapObjectType.None)
                    continue;
                (int x, int z) = customData.GetPosByIndex(i);
                Gizmos.color = customData.Grid[i].objectType switch
                {
                    MapObjectType.Ground => Color.yellow,
                    MapObjectType.Block  => Color.red,
                    MapObjectType.Water  => Color.blue,
                    MapObjectType.Shrub  => Color.green,
                    _                    => Color.white,
                };
                Gizmos.DrawCube(new Vector3(x / 2f + 0.25f, 0, z / 2f + 0.25f), new Vector3(0.49f, 0.1f, 0.49f));
            }
        }
    }

}