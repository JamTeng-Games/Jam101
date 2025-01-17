using System.IO;
using UnityEngine;

namespace Jam.Editor_
{

    public static class BulletAutoGenDefine
    {
        public static readonly string BulletSOPath_Relative = "Assets/Res_Nopack/SO/Bullet/";
        public static readonly string BulletSOPath_Abs = Application.dataPath + "/Res_Nopack/SO/Bullet/";

        public static readonly string BulletTypePath = Application.dataPath + "/QuantumUser/Simulation/Graph/Skill/Enum/BulletType.cs";
        public static readonly string BulletQtnPath = Application.dataPath + "/QuantumUser/Simulation/Qtn/BulletQtn_AutoGen.qtn";
        public static readonly string BulletNodeFolder = Application.dataPath + "/QuantumUser/Simulation/Graph/Skill/Bullet/Gen/";
        public static readonly string BulletCmdFolder = Application.dataPath + "/QuantumUser/Simulation/Logic/Helper/BulletCmd/";
        public static readonly string BulletHelperPath = Application.dataPath + "/QuantumUser/Simulation/Logic/Helper/Gen/Bullet/Helper_Bullet_Gen.cs";

        public static readonly string BulletTypeTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Bullet/Template/BulletTypeTemplate.txt";
        public static readonly string BulletQtnTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Bullet/Template/BulletQtnTemplate.txt";
        public static readonly string BulletNodeTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Bullet/Template/BulletNodeTemplate.txt";
        public static readonly string BulletCmdTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Bullet/Template/BulletCmdTemplate.txt";
        public static readonly string BulletHelperTemplatePath = Application.dataPath + "/Script/Editor/Toolbox/Bullet/Template/BulletHelperTemplate.txt";

        public static readonly string BulletTypeTemplate = File.ReadAllText(BulletTypeTemplatePath);
        public static readonly string BulletQtnTemplate = File.ReadAllText(BulletQtnTemplatePath);
        public static readonly string BulletNodeTemplate = File.ReadAllText(BulletNodeTemplatePath);
        public static readonly string BulletCmdTemplate = File.ReadAllText(BulletCmdTemplatePath);
        public static readonly string BulletHelperTemplate = File.ReadAllText(BulletHelperTemplatePath);

        public const string __BULLET_TYPE__ = "__BULLET_TYPE__";               // Enum
        public const string __BLTM_UNION__ = "__BLTM_UNION__";                 // Qtn
        public const string __BLTM_INSTANCE__ = "__BLTM_INSTANCE__";           // Qtn
        public const string __BULLET_NAME__ = "__BULLET_NAME__";               // Cmd
        public const string __BULLET_CMD_HELPER__ = "__BULLET_CMD_HELPER__";   // helper
        public const string __BULLET_DESC_NAME__ = "__BULLET_DESC_NAME__";     // node
        public const string __BULLET_ARGS__ = "__BULLET_ARGS__";               // node
        public const string __BULLET_ARGS_ASSIGN__ = "__BULLET_ARGS_ASSIGN__"; // node

        public static string BulletCmdPath(string name)
        {
            return BulletCmdFolder + name + ".cs";
        }
    }

}