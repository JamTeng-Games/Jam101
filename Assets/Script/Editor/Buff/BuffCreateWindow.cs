using UnityEditor;
using UnityEngine;

namespace Jam.Editor_
{

    public class BuffCreateWindow : EditorWindow
    {
        public static void OpenWindow()
        {
            BuffCreateWindow window = GetWindow<BuffCreateWindow>();
            window.titleContent = new GUIContent("Buff管理");
            window.Show();
        }
    }

}