using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_
{

    using Define = BuffAutoGenDefine;

    public class BuffAutoGen
    {
        public static void GenBuffCode()
        {
            IEnumerable<BuffTemplateSO> allBuffSo = AssetDatabase.FindAssets("t:BuffTemplateSO")
                                                                 .Select(AssetDatabase.GUIDToAssetPath)
                                                                 .Select(AssetDatabase.LoadAssetAtPath<BuffTemplateSO>)
                                                                 .Where(so => so != null);
            var buffs = allBuffSo.OrderBy(s => s.name).ToList();
            GenBuffTypeCode(buffs);
            GenBuffQtnCode(buffs);
            GenBuffNodeCode(buffs);
            GenBuffCmdCode(buffs);
            GenBuffHelperCode(buffs);
            AssetDatabase.Refresh();
        }

        private static void GenBuffTypeCode(List<BuffTemplateSO> buffs)
        {
            string buffTypes = string.Join(",\n        ", buffs.Select(b => b.name));
            string fileContent = Define.BuffTypeTemplate.Replace(Define.__BUFF_TYPE__, buffTypes);
            BuffAutoGenUtils.SaveBuffType(fileContent);
        }

        private static void GenBuffQtnCode(List<BuffTemplateSO> buffs)
        {
            // union part
            string unionPart =
                string.Join(
                    "\n    ",
                    buffs.Select(b => $"BM_{b.name} {b.name};"));
            string fileContent = Define.BuffQtnTemplate.Replace(Define.__BM_UNION__, unionPart);

            // instance part
            string instancePart = string.Join("\n", buffs.Select(b =>
            {
                StringBuilder body = new StringBuilder();
                body.Append($"struct BM_{b.name}\n{{\n");
                if (b.args != null)
                {
                    // if (b.args.Count == 0)
                    //     body.Append("    bool placeHolder;  // placeHolder\n");

                    foreach (var arg in b.args)
                    {
                        if (arg.type == BuffArgType.None)
                            continue;
                        string typeStr = BuffAutoGenUtils.GetArgTypeStr(arg.type);
                        body.Append($"    {typeStr} {arg.argName};  // {arg.desc}\n");
                    }
                }
                body.Append("}\n");
                return body.ToString();
            }));
            fileContent = fileContent.Replace(Define.__BM_INSTANCE__, instancePart);
            BuffAutoGenUtils.SaveBuffQtn(fileContent);
        }

        private static void GenBuffNodeCode(List<BuffTemplateSO> buffs)
        {
            // Clear folder
            BuffAutoGenUtils.ClearNodeFolder();

            foreach (var buff in buffs)
            {
                string fileContent = Define.BuffNodeTemplate.Replace(Define.__BUFF_DESC_NAME__, $"\"{buff.descName}\"");
                fileContent = fileContent.Replace(Define.__BUFF_NAME__, buff.name);
                // args definition
                StringBuilder argsDef = new StringBuilder();
                foreach (var arg in buff.args)
                {
                    // attribute
                    argsDef.Append($"        [GraphDisplay(DisplayType.BothViews)] ");
                    argsDef.Append($"public {BuffAutoGenUtils.GetArgTypeStr(arg.type)} {arg.argName};\n");
                }
                fileContent = fileContent.Replace(Define.__BUFF_ARGS__, argsDef.ToString());

                // buff_type
                fileContent = fileContent.Replace(Define.__BUFF_TYPE__, buff.name);

                // buff_args_assign
                argsDef.Clear();
                foreach (var arg in buff.args)
                {
                    argsDef.Append($"            ");
                    argsDef.Append($"bm.{buff.name}->{arg.argName} = {arg.argName};\n");
                }
                fileContent = fileContent.Replace(Define.__BUFF_ARGS_ASSIGN__, argsDef.ToString());
                BuffAutoGenUtils.SaveBuffNode(buff.name, fileContent);
            }
        }

        private static void GenBuffCmdCode(List<BuffTemplateSO> buffs)
        {
            foreach (var buff in buffs)
            {
                string fileContent = Define.BuffCmdTemplate.Replace(Define.__BUFF_NAME__, buff.name);
                BuffAutoGenUtils.SaveBuffCmd(buff.name, fileContent);
            }
        }

        private static void GenBuffHelperCode(List<BuffTemplateSO> buffs)
        {
            StringBuilder cmdStr = new StringBuilder();
            foreach (var buff in buffs)
            {
                cmdStr.Append("                ");
                cmdStr.Append($"{{ (int)BuffType.{buff.name}, new BuffCmd_{buff.name}() }},\n");
            }
            string fileContent = Define.BuffHelperTemplate.Replace(Define.__BUFF_CMD_HELPER__, cmdStr.ToString());
            BuffAutoGenUtils.SaveBuffHelper(fileContent);
        }
    }

}