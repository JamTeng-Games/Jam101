using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_.UITool
{

    public class UIToolEntry
    {
        [MenuItem("GameObject/UITool/生成UI")]
        private static void GeneratePanelForGameObject()
        {
            if (Selection.activeGameObject.name.EndsWith("Panel"))
            {
                UIToolPanel.ProcessPanelPipeline();
            }
            else if (Selection.activeGameObject.name.EndsWith("Widget"))
            {
                UIToolWidget.ProcessWidgetPipeline();
            }
        }

        [MenuItem("Assets/UITool/生成UI")]
        private static void GeneratePanelForPrefab()
        {
            if (Selection.activeGameObject.name.EndsWith("Panel"))
            {
                UIToolPanel.ProcessPanelPipeline();
            }
            else if (Selection.activeGameObject.name.EndsWith("Widget"))
            {
                UIToolWidget.ProcessWidgetPipeline();
            }
        }

        [MenuItem("GameObject/UITool/生成UI", true)]
        private static bool ValidateGeneratePanelForGameObject()
        {
            if (Selection.activeGameObject == null)
                return false;

            if (!Selection.activeGameObject.name.EndsWith("Panel") &&
                !Selection.activeGameObject.name.EndsWith("Widget"))
            {
                return false;
            }

            return Selection.activeGameObject?.GetComponent<RectTransform>() != null;
        }

        [MenuItem("Assets/UITool/生成UI", true)]
        private static bool ValidateGeneratePanelForPrefab()
        {
            if (Selection.activeGameObject == null)
                return false;

            if (!Selection.activeGameObject.name.EndsWith("Panel") &&
                !Selection.activeGameObject.name.EndsWith("Widget"))
            {
                return false;
            }
            return Selection.activeGameObject?.GetComponent<RectTransform>() != null;
        }
    }

}