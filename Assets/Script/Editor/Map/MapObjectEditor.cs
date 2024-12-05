using Jam.Runtime;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_
{

    [CustomEditor(typeof(MapObject))]
    public class MapObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // 绘制默认的 Inspector 内容（包括 MapObject 的字段）
            DrawDefaultInspector();

            // 获取当前 MapObject 实例
            MapObject mapObject = (MapObject)target;

            // 添加一个自定义按钮，点击时执行 OnButtonClicked 方法
            if (GUILayout.Button("删除无用Collider"))
            {
                mapObject.RemoveBox();
            }
        }

        // 单位格移动
        private void OnSceneGUI()
        {
            Transform transform = ((MapObject)target).transform;

            // 获取当前物体的坐标
            Vector3 position = transform.position;

            // 将坐标强制为整数（以1米为单位）
            Vector3 snappedPosition = new Vector3(
                Mathf.Round(position.x),
                Mathf.Round(position.y),
                Mathf.Round(position.z)
            );

            // 如果物体的位置被改变了，更新物体的位置
            if (transform.position != snappedPosition)
            {
                Undo.RecordObject(transform, "Snap Position");
                transform.position = snappedPosition;
                ((MapObject)target).ResetMinMax();
            }
        }
    }

}