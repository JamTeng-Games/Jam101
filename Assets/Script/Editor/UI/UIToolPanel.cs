using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jam.Core;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_.UITool
{

    public class UIFieldInfo
    {
        public string type;
        public string name;
        public string transPath;

        public string FieldString()
        {
            return $"[SerializeField] private {type} _{name};";
        }

        public string ValidateString()
        {
            return $"_{name} = transform.Find(\"{transPath}\").GetComponent<{type}>();";
        }
    }

    public static class UIToolPanel
    {
        public static void ProcessPanelPipeline()
        {
            string panelFileName = GenPanelCode();
            GenPanelPrefab(panelFileName);
        }

        private static string GenPanelCode()
        {
            string panelName = Selection.activeGameObject.name.Replace("Panel", "");
            string panelFileName = panelName + "Panel";

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

            string viewFileContent = UIToolDefine.PanelViewTemplate.Replace(UIToolDefine.__PANEL_NAME__, panelName)
                                                 .Replace(UIToolDefine.__FIELD__, fieldBuilder.ToString())
                                                 .Replace(UIToolDefine.__VALIDATE__, variableBuilder.ToString());

            UIToolUtil.PanelSaveViewFile(panelFileName, viewFileContent);
            if (!File.Exists(UIToolDefine.PanelLogicSavePath(panelFileName)))
            {
                string logicFileContent =
                    UIToolDefine.PanelLogicTemplate.Replace(UIToolDefine.__PANEL_NAME__, panelName);
                UIToolUtil.PanelSaveLogicFile(panelFileName, logicFileContent);
            }
            AssetDatabase.Refresh();
            return panelFileName;
        }

        private static void GenPanelPrefab(string panelFileName)
        {
            string prefabPath = $"{UIToolDefine.PanelPrefabAssetPath}{panelFileName}.prefab";

            // 检查Prefab是否存在
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                Debug.Log($"Contains prefab {panelFileName}");
                // Prefab存在，检查是否包含脚本A
                MonoBehaviour scriptA = prefab.GetComponent(panelFileName) as MonoBehaviour;
                if (scriptA != null)
                {
                    // 刷新Prefab
                    EditorUtility.SetDirty(prefab);
                    Debug.Log($"Prefab '{panelFileName}' refreshed.");
                }
                else
                {
                    // 添加脚本A
                    Type panelType = Util.Assembly.GetType($"Jam.{panelFileName}");
                    if (panelType != null)
                    {
                        prefab.AddComponent(panelType);
                    }
                    else
                    {
                        Debug.Log($"No panel component found for {panelFileName}");
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