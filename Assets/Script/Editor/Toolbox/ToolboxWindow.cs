using System;
using NewGraph;
using Quantum;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_
{

    public class ToolboxWindow : OdinEditorWindow
    {
        [MenuItem("Tools/工具箱")]
        public static void OpenWindow()
        {
            ToolboxWindow window = GetWindow<ToolboxWindow>();
            window.titleContent = new GUIContent("工具箱");
            window.Show();
        }

        [Button("技能编辑器", ButtonSizes.Gigantic), HorizontalGroup("row1")]
        public void OpenSkill()
        {
            SkillWindow.OpenWindow();
        }

        [Button("Buff管理", ButtonSizes.Gigantic), HorizontalGroup("row2")]
        public void OpenBuff()
        {
            BuffWindow.OpenWindow();
        }

        [Button("子弹管理", ButtonSizes.Gigantic), HorizontalGroup("row2")]
        public void OpenBullet()
        {
            BulletWindow.OpenWindow();
        }

        [Button("地图烘焙", ButtonSizes.Gigantic), HorizontalGroup("row3")]
        public void OpenMapBaker()
        {
            MapBakerWindow.OpenWindow();
        }
    }

}