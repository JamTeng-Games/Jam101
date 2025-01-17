using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Jam.Cfg;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_
{

    using Define = BulletAutoGenDefine;
    using BulletArgType = BuffArgType;

    public class BulletAutoGen
    {
        public static void GenBulletCode()
        {
            IEnumerable<BulletTemplateSO> allBulletSo = AssetDatabase.FindAssets("t:BulletTemplateSO")
                                                                     .Select(AssetDatabase.GUIDToAssetPath)
                                                                     .Select(AssetDatabase.LoadAssetAtPath<BulletTemplateSO>)
                                                                     .Where(so => so != null);
            var bullets = allBulletSo.OrderBy(s => s.name).ToList();
            GenBulletTypeCode(bullets);
            GenBulletQtnCode(bullets);
            GenBulletNodeCode(bullets);
            GenBulletCmdCode(bullets);
            GenBulletHelperCode(bullets);
            AssetDatabase.Refresh();
        }

        private static void GenBulletTypeCode(List<BulletTemplateSO> bullets)
        {
            string bulletTypes = string.Join(",\n        ", bullets.Select(b => b.name));
            string fileContent = Define.BulletTypeTemplate.Replace(Define.__BULLET_TYPE__, bulletTypes);
            BulletAutoGenUtils.SaveBulletType(fileContent);
        }

        private static void GenBulletQtnCode(List<BulletTemplateSO> bullets)
        {
            // union part
            string unionPart =
                string.Join(
                    "\n    ",
                    bullets.Select(b => $"BM_{b.name} {b.name};"));
            string fileContent = Define.BulletQtnTemplate.Replace(Define.__BLTM_UNION__, unionPart);

            // instance part
            string instancePart = string.Join("\n", bullets.Select(b =>
            {
                StringBuilder body = new StringBuilder();
                body.Append($"struct BM_{b.name}\n{{\n");
                if (b.args != null)
                {
                    // if (b.args.Count == 0)
                    //     body.Append("    bool placeHolder;  // placeHolder\n");

                    foreach (var arg in b.args)
                    {
                        if (arg.type == BulletArgType.None)
                            continue;
                        string typeStr = BulletAutoGenUtils.GetArgTypeStr(arg.type);
                        body.Append($"    {typeStr} {arg.argName};  // {arg.desc}\n");
                    }
                }
                body.Append("}\n");
                return body.ToString();
            }));
            fileContent = fileContent.Replace(Define.__BLTM_INSTANCE__, instancePart);
            BulletAutoGenUtils.SaveBulletQtn(fileContent);
        }

        private static void GenBulletNodeCode(List<BulletTemplateSO> bullets)
        {
            // Clear folder
            // BulletAutoGenUtils.ClearNodeFolder();

            foreach (var bullet in bullets)
            {
                string fileContent = Define.BulletNodeTemplate.Replace(Define.__BULLET_DESC_NAME__, $"\"{bullet.descName}\"");
                fileContent = fileContent.Replace(Define.__BULLET_NAME__, bullet.name);
                // args definition
                StringBuilder argsDef = new StringBuilder();
                foreach (var arg in bullet.args)
                {
                    // attribute
                    argsDef.Append($"        [GraphDisplay(DisplayType.BothViews)] ");
                    argsDef.Append($"public {BulletAutoGenUtils.GetArgTypeStr(arg.type)} {arg.argName};\n");
                }
                fileContent = fileContent.Replace(Define.__BULLET_ARGS__, argsDef.ToString());

                // bullet_type
                fileContent = fileContent.Replace(Define.__BULLET_TYPE__, bullet.name);

                // bullet_args_assign
                argsDef.Clear();
                foreach (var arg in bullet.args)
                {
                    argsDef.Append($"            ");
                    argsDef.Append($"bm.{bullet.name}->{arg.argName} = {arg.argName};\n");
                }
                fileContent = fileContent.Replace(Define.__BULLET_ARGS_ASSIGN__, argsDef.ToString());
                BulletAutoGenUtils.SaveBulletNode(bullet.name, fileContent);
            }
        }

        private static void GenBulletCmdCode(List<BulletTemplateSO> bullets)
        {
            List<string> files = null;
            if (Directory.Exists(Define.BulletCmdFolder))
            {
                files = Directory.GetFiles(Define.BulletCmdFolder).Select(Path.GetFileNameWithoutExtension).ToList();
            }

            foreach (var bullet in bullets)
            {
                if (files != null && files.Contains($"BulletCmd_{bullet.name}"))
                    continue;

                string fileContent = Define.BulletCmdTemplate.Replace(Define.__BULLET_NAME__, bullet.name);
                BulletAutoGenUtils.SaveBulletCmd(bullet.name, fileContent);
            }
        }

        private static void GenBulletHelperCode(List<BulletTemplateSO> bullets)
        {
            StringBuilder cmdStr = new StringBuilder();
            foreach (var bullet in bullets)
            {
                cmdStr.Append("                ");
                cmdStr.Append($"{{ (int)BulletType.{bullet.name}, new BulletCmd_{bullet.name}() }},\n");
            }
            string fileContent = Define.BulletHelperTemplate.Replace(Define.__BULLET_CMD_HELPER__, cmdStr.ToString());
            BulletAutoGenUtils.SaveBulletHelper(fileContent);
        }
    }

}