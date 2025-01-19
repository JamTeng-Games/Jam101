using System.IO;
using UnityEngine;

namespace Jam.Editor_
{

    public static class AoeAutoGenDefine
    {
        public static readonly string AoeSOPath_Relative = "Assets/Res_Nopack/SO/Aoe/";
        public static readonly string AoeSOPath_Abs = Application.dataPath + "/Res_Nopack/SO/Aoe/";

        public static readonly string AoeTypePath = Application.dataPath + "/QuantumUser/Simulation/Graph/Skill/Enum/AoeType.cs";
        public static readonly string AoeQtnPath = Application.dataPath + "/QuantumUser/Simulation/Qtn/AoeQtn_AutoGen.qtn";
        public static readonly string AoeNodeFolder = Application.dataPath + "/QuantumUser/Simulation/Graph/Skill/Aoe/Gen/";
        public static readonly string AoeCmdFolder = Application.dataPath + "/QuantumUser/Simulation/Logic/Helper/AoeCmd/";
        public static readonly string AoeHelperPath = Application.dataPath + "/QuantumUser/Simulation/Logic/Helper/Gen/Aoe/Helper_Aoe_Gen.cs";

        public static readonly string AoeTypeTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Aoe/Template/AoeTypeTemplate.txt";
        public static readonly string AoeQtnTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Aoe/Template/AoeQtnTemplate.txt";
        public static readonly string AoeNodeTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Aoe/Template/AoeNodeTemplate.txt";
        public static readonly string AoeCmdTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Aoe/Template/AoeCmdTemplate.txt";
        public static readonly string AoeHelperTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Aoe/Template/AoeHelperTemplate.txt";

        public static readonly string AoeTypeTemplate = File.ReadAllText(AoeTypeTemplatePath);
        public static readonly string AoeQtnTemplate = File.ReadAllText(AoeQtnTemplatePath);
        public static readonly string AoeNodeTemplate = File.ReadAllText(AoeNodeTemplatePath);
        public static readonly string AoeCmdTemplate = File.ReadAllText(AoeCmdTemplatePath);
        public static readonly string AoeHelperTemplate = File.ReadAllText(AoeHelperTemplatePath);

        public const string __AOE_TYPE__ = "__AOE_TYPE__";               // Enum
        public const string __AOEM_UNION__ = "__AOEM_UNION__";                 // Qtn
        public const string __AOEM_INSTANCE__ = "__AOEM_INSTANCE__";           // Qtn
        public const string __AOE_NAME__ = "__AOE_NAME__";               // Cmd
        public const string __AOE_CMD_HELPER__ = "__AOE_CMD_HELPER__";   // helper
        public const string __AOE_DESC_NAME__ = "__AOE_DESC_NAME__";     // node
        public const string __AOE_ARGS__ = "__AOE_ARGS__";               // node
        public const string __AOE_ARGS_ASSIGN__ = "__AOE_ARGS_ASSIGN__"; // node

        public static string AoeCmdPath(string name)
        {
            return AoeCmdFolder + name + ".cs";
        }
    }

}