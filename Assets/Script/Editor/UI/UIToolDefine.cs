using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Jam.Editor_.UITool
{

    public static class UIToolDefine
    {
        public static readonly List<string> ControlPrefix = new List<string>
        {
            "btn_",
            "img_",
            "rawimg_",
            "txt_",
            "input_",
            "toggle_",
            "slider_",
            "scroll_",
            "dropdown_",
            "node_",
        };

        public static readonly Dictionary<string, string> ControlTypeDic = new Dictionary<string, string>
        {
            { "btn_", "Button" },
            { "img_", "Image" },
            { "rawimg_", "RawImage" },
            { "txt_", "TextMeshProUGUI" },
            { "input_", "TMP_InputField" },
            { "toggle_", "Toggle" },
            { "slider_", "Slider" },
            { "scroll_", "ScrollRect" },
            { "dropdown_", "TMP_Dropdown" },
            { "node_", "Transform" }
        };

        public const string __PANEL_NAME__ = "__PANEL_NAME__";
        public const string __WIDGET_NAME__ = "__WIDGET_NAME__";
        public const string __FIELD__ = "__FIELD__";
        public const string __VALIDATE__ = "__VALIDATE__";

        public static readonly string PanelViewTemplatePath =
            Application.dataPath + $"/Script/Editor/UI/Template/PanelViewTemplate.txt";
        public static readonly string PanelLogicTemplatePath =
            Application.dataPath + $"/Script/Editor/UI/Template/PanelLogicTemplate.txt";

        public static readonly string WidgetViewTemplatePath =
            Application.dataPath + $"/Script/Editor/UI/Template/WidgetViewTemplate.txt";
        public static readonly string WidgetLogicTemplatePath =
            Application.dataPath + $"/Script/Editor/UI/Template/WidgetLogicTemplate.txt";

        public static readonly string PanelViewTemplate = File.ReadAllText(PanelViewTemplatePath);
        public static readonly string PanelLogicTemplate = File.ReadAllText(PanelLogicTemplatePath);

        public static readonly string WidgetViewTemplate = File.ReadAllText(WidgetViewTemplatePath);
        public static readonly string WidgetLogicTemplate = File.ReadAllText(WidgetLogicTemplatePath);

        public static string PanelViewSavePath(string panelFileName) =>
            Application.dataPath + $"/Script/Runtime/UI/View/Panel/{panelFileName}View.cs";

        public static string PanelLogicSavePath(string panelFileName) =>
            Application.dataPath + $"/Script/Runtime/UI/Panel/{panelFileName}.cs";

        public static string WidgetViewSavePath(string widgetFileName) =>
            Application.dataPath + $"/Script/Runtime/UI/View/Widget/{widgetFileName}View.cs";

        public static string WidgetLogicSavePath(string widgetFileName) =>
            Application.dataPath + $"/Script/Runtime/UI/Widget/{widgetFileName}.cs";

        
        public static readonly string PanelPrefabAbsolutePath = Application.dataPath + "/Res/UI/Panel/";
        public static readonly string PanelPrefabAssetPath = "Assets/Res/UI/Panel/";
        
        public static readonly string WidgetPrefabAbsolutePath = Application.dataPath + "/Res/UI/Widget/";
        public static readonly string WidgetPrefabAssetPath = "Assets/Res/UI/Widget/";
    }

}