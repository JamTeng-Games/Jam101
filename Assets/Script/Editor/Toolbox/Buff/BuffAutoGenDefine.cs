using System.IO;
using UnityEngine;

namespace Jam.Editor_
{

    public static class BuffAutoGenDefine
    {
        public static readonly string BuffSOPath_Relative = "Assets/Res_Nopack/SO/Buff/";
        public static readonly string BuffSOPath_Abs = Application.dataPath + "/Res_Nopack/SO/Buff/";

        public static readonly string BuffTypePath = Application.dataPath + "/QuantumUser/Simulation/Graph/Skill/Enum/BuffType.cs";
        public static readonly string BuffQtnPath = Application.dataPath + "/QuantumUser/Simulation/Qtn/BuffQtn_AutoGen.qtn";
        public static readonly string BuffNodeFolder = Application.dataPath + "/QuantumUser/Simulation/Graph/Skill/Buff/Gen/";
        public static readonly string BuffCmdFolder = Application.dataPath + "/QuantumUser/Simulation/Logic/Helper/BuffCmd/";
        public static readonly string BuffHelperPath = Application.dataPath + "/QuantumUser/Simulation/Logic/Helper/Gen/Buff/Helper_Buff_Gen.cs";

        public static readonly string BuffTypeTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Buff/Template/BuffTypeTemplate.txt";
        public static readonly string BuffQtnTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Buff/Template/BuffQtnTemplate.txt";
        public static readonly string BuffNodeTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Buff/Template/BuffNodeTemplate.txt";
        public static readonly string BuffCmdTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Buff/Template/BuffCmdTemplate.txt";
        public static readonly string BuffHelperTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Buff/Template/BuffHelperTemplate.txt";

        public static readonly string BuffTypeTemplate = File.ReadAllText(BuffTypeTemplatePath);
        public static readonly string BuffQtnTemplate = File.ReadAllText(BuffQtnTemplatePath);
        public static readonly string BuffNodeTemplate = File.ReadAllText(BuffNodeTemplatePath);
        public static readonly string BuffCmdTemplate = File.ReadAllText(BuffCmdTemplatePath);
        public static readonly string BuffHelperTemplate = File.ReadAllText(BuffHelperTemplatePath);

        public const string __BUFF_TYPE__ = "__BUFF_TYPE__";             // Enum
        public const string __BM_UNION__ = "__BM_UNION__";               // Qtn
        public const string __BM_INSTANCE__ = "__BM_INSTANCE__";         // Qtn
        public const string __BUFF_NAME__ = "__BUFF_NAME__";             // Cmd
        public const string __BUFF_CMD_HELPER__ = "__BUFF_CMD_HELPER__"; // helper
        public const string __BUFF_DESC_NAME__ = "__BUFF_DESC_NAME__";      // node
        public const string __BUFF_ARGS__ = "__BUFF_ARGS__";                // node
        public const string __BUFF_ARGS_ASSIGN__ = "__BUFF_ARGS_ASSIGN__";  // node

        public static string BuffCmdPath(string name)
        {
            return BuffCmdFolder + name + ".cs";
        }
    }

}