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

    using Define = AoeAutoGenDefine;
    using AoeArgType = BuffArgType;

    public class AoeAutoGen
    {
        public static void GenAoeCode()
        {
            IEnumerable<AoeTemplateSO> allAoeSo = AssetDatabase.FindAssets("t:AoeTemplateSO")
                                                                     .Select(AssetDatabase.GUIDToAssetPath)
                                                                     .Select(AssetDatabase.LoadAssetAtPath<AoeTemplateSO>)
                                                                     .Where(so => so != null);
            var aoes = allAoeSo.OrderBy(s => s.name).ToList();
            GenAoeTypeCode(aoes);
            GenAoeQtnCode(aoes);
            GenAoeNodeCode(aoes);
            GenAoeCmdCode(aoes);
            GenAoeHelperCode(aoes);
            AssetDatabase.Refresh();
        }

        private static void GenAoeTypeCode(List<AoeTemplateSO> aoes)
        {
            string aoeTypes = string.Join(",\n        ", aoes.Select(b => b.name));
            string fileContent = Define.AoeTypeTemplate.Replace(Define.__AOE_TYPE__, aoeTypes);
            AoeAutoGenUtils.SaveAoeType(fileContent);
        }

        private static void GenAoeQtnCode(List<AoeTemplateSO> aoes)
        {
            // union part
            string unionPart =
                string.Join(
                    "\n    ",
                    aoes.Select(b => $"AOEM_{b.name} {b.name};"));
            string fileContent = Define.AoeQtnTemplate.Replace(Define.__AOEM_UNION__, unionPart);

            // instance part
            string instancePart = string.Join("\n", aoes.Select(b =>
            {
                StringBuilder body = new StringBuilder();
                body.Append($"struct AOEM_{b.name}\n{{\n");
                if (b.args != null)
                {
                    // if (b.args.Count == 0)
                    //     body.Append("    bool placeHolder;  // placeHolder\n");

                    foreach (var arg in b.args)
                    {
                        if (arg.type == AoeArgType.None)
                            continue;
                        string typeStr = AoeAutoGenUtils.GetArgTypeStr(arg.type);
                        body.Append($"    {typeStr} {arg.argName};  // {arg.desc}\n");
                    }
                }
                body.Append("}\n");
                return body.ToString();
            }));
            fileContent = fileContent.Replace(Define.__AOEM_INSTANCE__, instancePart);
            AoeAutoGenUtils.SaveAoeQtn(fileContent);
        }

        private static void GenAoeNodeCode(List<AoeTemplateSO> aoes)
        {
            // Clear folder
            // AoeAutoGenUtils.ClearNodeFolder();

            foreach (var aoe in aoes)
            {
                string fileContent = Define.AoeNodeTemplate.Replace(Define.__AOE_DESC_NAME__, $"\"{aoe.descName}\"");
                fileContent = fileContent.Replace(Define.__AOE_NAME__, aoe.name);
                // args definition
                StringBuilder argsDef = new StringBuilder();
                foreach (var arg in aoe.args)
                {
                    // attribute
                    argsDef.Append($"        [GraphDisplay(DisplayType.BothViews)] ");
                    argsDef.Append($"public {AoeAutoGenUtils.GetArgTypeStr(arg.type)} {arg.argName};\n");
                }
                fileContent = fileContent.Replace(Define.__AOE_ARGS__, argsDef.ToString());

                // aoe_type
                fileContent = fileContent.Replace(Define.__AOE_TYPE__, aoe.name);

                // aoe_args_assign
                argsDef.Clear();
                foreach (var arg in aoe.args)
                {
                    argsDef.Append($"            ");
                    argsDef.Append($"aoem.{aoe.name}->{arg.argName} = {arg.argName};\n");
                }
                fileContent = fileContent.Replace(Define.__AOE_ARGS_ASSIGN__, argsDef.ToString());
                AoeAutoGenUtils.SaveAoeNode(aoe.name, fileContent);
            }
        }

        private static void GenAoeCmdCode(List<AoeTemplateSO> aoes)
        {
            List<string> files = null;
            if (Directory.Exists(Define.AoeCmdFolder))
            {
                files = Directory.GetFiles(Define.AoeCmdFolder).Select(Path.GetFileNameWithoutExtension).ToList();
            }

            foreach (var aoe in aoes)
            {
                if (files != null && files.Contains($"AoeCmd_{aoe.name}"))
                    continue;

                string fileContent = Define.AoeCmdTemplate.Replace(Define.__AOE_NAME__, aoe.name);
                AoeAutoGenUtils.SaveAoeCmd(aoe.name, fileContent);
            }
        }

        private static void GenAoeHelperCode(List<AoeTemplateSO> aoes)
        {
            StringBuilder cmdStr = new StringBuilder();
            foreach (var aoe in aoes)
            {
                cmdStr.Append("                ");
                cmdStr.Append($"{{ (int)AoeType.{aoe.name}, new AoeCmd_{aoe.name}() }},\n");
            }
            string fileContent = Define.AoeHelperTemplate.Replace(Define.__AOE_CMD_HELPER__, cmdStr.ToString());
            AoeAutoGenUtils.SaveAoeHelper(fileContent);
        }
    }

}