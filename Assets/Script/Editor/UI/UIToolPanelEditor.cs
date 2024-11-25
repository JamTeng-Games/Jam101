using Jam.Runtime.UI_;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_.UITool
{

    [CustomEditor(typeof(CanvasRenderer), true)]
    public class UIToolPanelEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CanvasRenderer go = (CanvasRenderer)target;
            if (go.name.EndsWith("Panel") && go.GetComponent<UIPanel>() == null)
            {
                if (GUILayout.Button("自动添加Panel组件"))
                {
                    UIToolUtil.AddUIComponentToPrefab();
                }
            }

            if (go.name.EndsWith("Widget") && go.GetComponent<UIWidget>() == null)
            {
                if (GUILayout.Button("自动添加Widget组件"))
                {
                    UIToolUtil.AddUIComponentToPrefab();
                }
            }
        }
    }

}