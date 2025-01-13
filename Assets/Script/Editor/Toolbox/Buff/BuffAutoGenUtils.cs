using System.IO;
using System.Text;
using Jam.Cfg;

namespace Jam.Editor_
{

    public class BuffAutoGenUtils
    {
        public static void SaveBuffType(string content)
        {
            File.WriteAllText(BuffAutoGenDefine.BuffTypePath, content, Encoding.UTF8);
        }

        public static void SaveBuffQtn(string content)
        {
            File.WriteAllText(BuffAutoGenDefine.BuffQtnPath, content, Encoding.UTF8);
        }

        public static void SaveBuffNode(string buffName, string content)
        {
            File.WriteAllText($"{BuffAutoGenDefine.BuffNodeFolder}Buff{buffName}Node.cs", content, Encoding.UTF8);
        }

        public static void SaveBuffCmd(string buffName, string content)
        {
            File.WriteAllText($"{BuffAutoGenDefine.BuffCmdFolder}BuffCmd_{buffName}.cs", content, Encoding.UTF8);
        }

        public static void SaveBuffHelper(string content)
        {
            File.WriteAllText($"{BuffAutoGenDefine.BuffHelperPath}", content, Encoding.UTF8);
        }

        public static void ClearNodeFolder()
        {
            if (Directory.Exists(BuffAutoGenDefine.BuffNodeFolder))
            {
                Directory.Delete(BuffAutoGenDefine.BuffNodeFolder, true);
            }
            Directory.CreateDirectory(BuffAutoGenDefine.BuffNodeFolder);
        }

        public static string GetArgTypeStr(BuffArgType argType)
        {
            string typeStr = argType switch
            {
                BuffArgType.Bool   => "bool",
                BuffArgType.Int8   => "sbyte",
                BuffArgType.Int16  => "short",
                BuffArgType.Int32  => "int",
                BuffArgType.Int64  => "long",
                BuffArgType.UInt8  => "byte",
                BuffArgType.UInt16 => "ushort",
                BuffArgType.UInt32 => "uint",
                BuffArgType.UInt64 => "ulong",
                BuffArgType.Fp     => "FP",
                BuffArgType.Vec2   => "FPVector2",
                BuffArgType.Vec3   => "FPVector3",
                BuffArgType.Str    => "QString<64>",
                _                  => ""
            };
            return typeStr;
        }
    }

}