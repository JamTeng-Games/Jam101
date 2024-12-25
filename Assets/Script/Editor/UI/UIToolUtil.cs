using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jam.Core;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_.UITool
{

    public static class UIToolUtil
    {
        public static void PanelSaveViewFile(string panelFileName, string content)
        {
            File.WriteAllText(UIToolDefine.PanelViewSavePath(panelFileName), content, Encoding.UTF8);
        }

        public static void PanelSaveLogicFile(string panelFileName, string content)
        {
            File.WriteAllText(UIToolDefine.PanelLogicSavePath(panelFileName), content, Encoding.UTF8);
        }

        public static void WidgetSaveViewFile(string widgetFileName, string content)
        {
            File.WriteAllText(UIToolDefine.WidgetViewSavePath(widgetFileName), content, Encoding.UTF8);
        }

        public static void WidgetSaveLogicFile(string widgetFileName, string content)
        {
            File.WriteAllText(UIToolDefine.WidgetLogicSavePath(widgetFileName), content, Encoding.UTF8);
        }

        public static void GenerateFieldInfo(List<UIFieldInfo> fieldInfos, Transform root)
        {
            fieldInfos.Clear();
            GenerateFieldInfoImpl(fieldInfos, null, root);
        }

        private static void GenerateFieldInfoImpl(List<UIFieldInfo> fieldInfos, string path, Transform root)
        {
            foreach (var prefix in UIToolDefine.ControlPrefix)
            {
                if (root.name.StartsWith(prefix))
                {
                    fieldInfos.Add(new UIFieldInfo
                    {
                        type = UIToolDefine.ControlTypeDic[prefix],
                        name = root.name,
                        transPath = path
                    });
                    break;
                }
            }

            for (int i = 0; i < root.childCount; i++)
            {
                Transform child = root.GetChild(i);
                if (path == null)
                {
                    GenerateFieldInfoImpl(fieldInfos, $"{child.name}", child);
                }
                else
                {
                    GenerateFieldInfoImpl(fieldInfos, $"{path}/{child.name}", child);
                }
            }
        }

        public static void AddUIComponentToPrefab()
        {
            Type panelType = Utils.Assembly.GetType($"Jam.Runtime.UI_.{Selection.activeGameObject.name}");
            if (panelType == null)
            {
                Debug.LogError($"No panel component found for {Selection.activeGameObject.name}");
                return;
            }
            GameObject selectedObject = Selection.activeGameObject;
            selectedObject.AddComponent(panelType);
        }
    }

}