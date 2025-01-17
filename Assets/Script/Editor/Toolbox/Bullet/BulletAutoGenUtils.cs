using System.IO;
using System.Text;
using Jam.Cfg;

namespace Jam.Editor_
{

    using BulletArgType = BuffArgType;

    public class BulletAutoGenUtils
    {
        public static void SaveBulletType(string content)
        {
            File.WriteAllText(BulletAutoGenDefine.BulletTypePath, content, Encoding.UTF8);
        }

        public static void SaveBulletQtn(string content)
        {
            File.WriteAllText(BulletAutoGenDefine.BulletQtnPath, content, Encoding.UTF8);
        }

        public static void SaveBulletNode(string bulletName, string content)
        {
            File.WriteAllText($"{BulletAutoGenDefine.BulletNodeFolder}Bullet{bulletName}Node.cs", content, Encoding.UTF8);
        }

        public static void SaveBulletCmd(string bulletName, string content)
        {
            File.WriteAllText($"{BulletAutoGenDefine.BulletCmdFolder}BulletCmd_{bulletName}.cs", content, Encoding.UTF8);
        }

        public static void SaveBulletHelper(string content)
        {
            File.WriteAllText($"{BulletAutoGenDefine.BulletHelperPath}", content, Encoding.UTF8);
        }

        public static void ClearNodeFolder()
        {
            if (Directory.Exists(BulletAutoGenDefine.BulletNodeFolder))
            {
                Directory.Delete(BulletAutoGenDefine.BulletNodeFolder, true);
            }
            Directory.CreateDirectory(BulletAutoGenDefine.BulletNodeFolder);
        }

        public static string GetArgTypeStr(BulletArgType argType)
        {
            string typeStr = argType switch
            {
                BulletArgType.Bool   => "bool",
                BulletArgType.Int8   => "sbyte",
                BulletArgType.Int16  => "short",
                BulletArgType.Int32  => "int",
                BulletArgType.Int64  => "long",
                BulletArgType.UInt8  => "byte",
                BulletArgType.UInt16 => "ushort",
                BulletArgType.UInt32 => "uint",
                BulletArgType.UInt64 => "ulong",
                BulletArgType.Fp     => "FP",
                BulletArgType.Vec2   => "FPVector2",
                BulletArgType.Vec3   => "FPVector3",
                BulletArgType.Str    => "QString<64>",
                _                    => ""
            };
            return typeStr;
        }
    }

}