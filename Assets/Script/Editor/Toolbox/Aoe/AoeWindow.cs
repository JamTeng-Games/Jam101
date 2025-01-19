using System.Collections.Generic;
using Jam.Cfg;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_
{

    public class AoeWindow : OdinMenuEditorWindow
    {
        private NewAoeData _newAoeData;
        private bool _isDirty = false;

        public static void OpenWindow()
        {
            AoeWindow window = GetWindow<AoeWindow>();
            window.titleContent = new GUIContent("Aoe管理");
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
                AutoGenAoeCode();

            _newAoeData?.Dispose();
            _newAoeData = null;
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = true;
            tree.Config.DrawSearchToolbar = true;

            _newAoeData = new NewAoeData(this);

            tree.Add("创建新Aoe", _newAoeData);
            tree.AddAllAssetsAtPath("Aoes", AoeAutoGenDefine.AoeSOPath_Relative, typeof(AoeTemplateSO));

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
                    AutoGenAoeCode();
                }

                if (SirenixEditorGUI.ToolbarButton("删除选中"))
                {
                    Debug.Log("Delete Aoe");
                    DeleteAoe(selected.SelectedValues);
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        private void DeleteAoe(IEnumerable<object> aoeAssets)
        {
            bool deleteOne = false;
            foreach (var asset in aoeAssets)
            {
                if (asset is AoeTemplateSO aoeAsset)
                {
                    string path = AssetDatabase.GetAssetPath(aoeAsset);
                    AssetDatabase.DeleteAsset(path);
                    deleteOne = true;
                }
            }
            AssetDatabase.SaveAssets();
            _isDirty |= deleteOne;
        }

        private void AutoGenAoeCode()
        {
            Debug.Log("AutoGenAoeCode");
            AoeAutoGen.GenAoeCode();
        }
    }

}