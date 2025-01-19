using System.IO;
using System.Text;
using Jam.Cfg;

namespace Jam.Editor_
{

    using AoeArgType = BuffArgType;

    public class AoeAutoGenUtils
    {
        public static void SaveAoeType(string content)
        {
            File.WriteAllText(AoeAutoGenDefine.AoeTypePath, content, Encoding.UTF8);
        }

        public static void SaveAoeQtn(string content)
        {
            File.WriteAllText(AoeAutoGenDefine.AoeQtnPath, content, Encoding.UTF8);
        }

        public static void SaveAoeNode(string aoeName, string content)
        {
            File.WriteAllText($"{AoeAutoGenDefine.AoeNodeFolder}Aoe{aoeName}Node.cs", content, Encoding.UTF8);
        }

        public static void SaveAoeCmd(string aoeName, string content)
        {
            File.WriteAllText($"{AoeAutoGenDefine.AoeCmdFolder}AoeCmd_{aoeName}.cs", content, Encoding.UTF8);
        }

        public static void SaveAoeHelper(string content)
        {
            File.WriteAllText($"{AoeAutoGenDefine.AoeHelperPath}", content, Encoding.UTF8);
        }

        public static void ClearNodeFolder()
        {
            if (Directory.Exists(AoeAutoGenDefine.AoeNodeFolder))
            {
                Directory.Delete(AoeAutoGenDefine.AoeNodeFolder, true);
            }
            Directory.CreateDirectory(AoeAutoGenDefine.AoeNodeFolder);
        }

        public static string GetArgTypeStr(AoeArgType argType)
        {
            string typeStr = argType switch
            {
                AoeArgType.Bool   => "bool",
                AoeArgType.Int8   => "sbyte",
                AoeArgType.Int16  => "short",
                AoeArgType.Int32  => "int",
                AoeArgType.Int64  => "long",
                AoeArgType.UInt8  => "byte",
                AoeArgType.UInt16 => "ushort",
                AoeArgType.UInt32 => "uint",
                AoeArgType.UInt64 => "ulong",
                AoeArgType.Fp     => "FP",
                AoeArgType.Vec2   => "FPVector2",
                AoeArgType.Vec3   => "FPVector3",
                AoeArgType.Str    => "QString<64>",
                _                 => ""
            };
            return typeStr;
        }
    }

}