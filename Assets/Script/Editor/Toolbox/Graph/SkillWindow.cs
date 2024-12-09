using NewGraph;
using Quantum;
using UnityEditor;

namespace Jam.Editor_
{

    public class SkillWindow
    {
        /// <summary>
        /// Here you can customize where the graph window menu should appear
        /// </summary>
        // [MenuItem("Tools/技能编辑器")]
        // static void OpenWindow() {
        // 	// Here you can define what type of graph model you would like to use.
        // 	GraphWindow.InitializeWindowBase(typeof(ScriptableGraphModel));
        // }
        public static void OpenWindow()
        {
            // Here you can define what type of graph model you would like to use.
            GraphWindow.InitializeWindowBase(typeof(AssetObjectGraphModel));
        }
    }

}