using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jam.Core;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_.UITool
{

    public static class UIToolWidget
    {
        public static void ProcessWidgetPipeline()
        {
            string widgetFileName = GenerateWidgetCode();
            GenerateWidgetPrefab(widgetFileName);
        }

        private static string GenerateWidgetCode()
        {
            string widgetName = Selection.activeGameObject.name.Replace("Widget", "");
            string widgetFileName = widgetName + "Widget";

            List<UIFieldInfo> fieldInfos = new List<UIFieldInfo>(64);
            UIToolUtil.GenerateFieldInfo(fieldInfos, Selection.activeGameObject.transform);

            StringBuilder fieldBuilder = new StringBuilder();
            StringBuilder variableBuilder = new StringBuilder();
            for (int i = 0; i < fieldInfos.Count; i++)
            {
                var fieldInfo = fieldInfos[i];
                if (i != fieldInfos.Count - 1)
                {
                    fieldBuilder.AppendLine($"        {fieldInfo.FieldString()}");
                    variableBuilder.AppendLine($"            {fieldInfo.ValidateString()}");
                }
                else
                {
                    fieldBuilder.Append($"        {fieldInfo.FieldString()}");
                    variableBuilder.Append($"            {fieldInfo.ValidateString()}");
                }
            }

            string viewFileContent = UIToolDefine.WidgetViewTemplate.Replace(UIToolDefine.__WIDGET_NAME__, widgetName)
                                                 .Replace(UIToolDefine.__FIELD__, fieldBuilder.ToString())
                                                 .Replace(UIToolDefine.__VALIDATE__, variableBuilder.ToString());

            UIToolUtil.WidgetSaveViewFile(widgetFileName, viewFileContent);
            if (!File.Exists(UIToolDefine.WidgetLogicSavePath(widgetFileName)))
            {
                string logicFileContent =
                    UIToolDefine.WidgetLogicTemplate.Replace(UIToolDefine.__WIDGET_NAME__, widgetName);
                UIToolUtil.WidgetSaveLogicFile(widgetFileName, logicFileContent);
            }
            AssetDatabase.Refresh();
            return widgetFileName;
        }

        private static void GenerateWidgetPrefab(string widgetFileName)
        {
            string prefabPath = $"{UIToolDefine.WidgetPrefabAssetPath}{widgetFileName}.prefab";

            // 检查Prefab是否存在
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                Debug.Log($"Contains prefab {widgetFileName}");
                // Prefab存在，检查是否包含脚本A
                MonoBehaviour scriptA = prefab.GetComponent(widgetFileName) as MonoBehaviour;
                if (scriptA != null)
                {
                    // 刷新Prefab
                    EditorUtility.SetDirty(prefab);
                    Debug.Log($"Prefab '{widgetFileName}' refreshed.");
                }
                else
                {
                    // 添加脚本A
                    Type widgetType = Util.Assembly.GetType($"Jam.{widgetFileName}");
                    if (widgetType != null)
                    {
                        prefab.AddComponent(widgetType);
                    }
                    else
                    {
                        Debug.Log($"No widget component found for {widgetFileName}");
                    }
                }

                // 选择该Prefab
                Selection.activeObject = prefab;
            }
            else
            {
                // Prefab不存在，创建新的Prefab
                GameObject selectedObject = Selection.activeGameObject;
                if (selectedObject != null)
                {
                    PrefabUtility.SaveAsPrefabAsset(selectedObject, prefabPath);
                }
                else
                {
                    Debug.LogWarning("No GameObject selected to create a prefab from.");
                }
            }
        }
    }

}