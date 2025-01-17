using System.Collections.Generic;
using Jam.Cfg;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_
{

    public class BulletWindow : OdinMenuEditorWindow
    {
        private NewBulletData _newBulletData;
        private bool _isDirty = false;

        public static void OpenWindow()
        {
            BulletWindow window = GetWindow<BulletWindow>();
            window.titleContent = new GUIContent("子弹管理");
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
                AutoGenBulletCode();

            _newBulletData?.Dispose();
            _newBulletData = null;
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = true;
            tree.Config.DrawSearchToolbar = true;

            _newBulletData = new NewBulletData(this);

            tree.Add("创建新子弹", _newBulletData);
            tree.AddAllAssetsAtPath("Bullets", BulletAutoGenDefine.BulletSOPath_Relative, typeof(BulletTemplateSO));

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
                    AutoGenBulletCode();
                }

                if (SirenixEditorGUI.ToolbarButton("删除选中"))
                {
                    Debug.Log("Delete Bullet");
                    DeleteBullet(selected.SelectedValues);
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        private void DeleteBullet(IEnumerable<object> bulletAssets)
        {
            bool deleteOne = false;
            foreach (var asset in bulletAssets)
            {
                if (asset is BulletTemplateSO bulletAsset)
                {
                    string path = AssetDatabase.GetAssetPath(bulletAsset);
                    AssetDatabase.DeleteAsset(path);
                    deleteOne = true;
                }
            }
            AssetDatabase.SaveAssets();
            _isDirty |= deleteOne;
        }

        private void AutoGenBulletCode()
        {
            Debug.Log("AutoGenBulletCode");
            BulletAutoGen.GenBulletCode();
        }
    }

}