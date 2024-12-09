using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_
{

    public class BuffWindow : OdinMenuEditorWindow
    {
        private NewBuffData _newBuffData;
        private bool _isDirty = false;

        public static void OpenWindow()
        {
            BuffWindow window = GetWindow<BuffWindow>();
            window.titleContent = new GUIContent("Buff管理");
            window.Show();
        }

        public void MarkDirty()
        {
            _isDirty = true;
        }

        public void ClearDirty()
        {
            _isDirty = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_isDirty)
                AutoGenBuffCode();

            _newBuffData.Dispose();
            _newBuffData = null;
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = true;
            tree.Config.DrawSearchToolbar = true;

            _newBuffData = new NewBuffData(this);

            tree.Add("创建新Buff", _newBuffData);
            tree.AddAllAssetsAtPath("Buffs", BuffAutoGenDefine.BuffSOPath_Relative, typeof(BuffTemplateSO));

            tree.EnumerateTree()
                .AddThumbnailIcons()
                .SortMenuItemsByName();

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            OdinMenuTreeSelection selected = this.MenuTree.Selection;
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();
                if (SirenixEditorGUI.ToolbarButton("代码生成"))
                {
                    AutoGenBuffCode();
                }

                if (SirenixEditorGUI.ToolbarButton("删除选中"))
                {
                    Debug.Log("Delete Buff");
                    DeleteBuff(selected.SelectedValues);
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        private void DeleteBuff(IEnumerable<object> buffAssets)
        {
            bool deleteOne = false;
            foreach (var asset in buffAssets)
            {
                if (asset is BuffTemplateSO buffAsset)
                {
                    string path = AssetDatabase.GetAssetPath(buffAsset);
                    AssetDatabase.DeleteAsset(path);
                    deleteOne = true;
                }
            }
            AssetDatabase.SaveAssets();
            _isDirty |= deleteOne;
        }

        private void AutoGenBuffCode()
        {
            Debug.Log("AutoGenBuffCode");
            BuffAutoGen.GenBuffCode();
        }
    }

}