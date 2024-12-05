using UnityEditor;
using UnityEditor.Callbacks;

namespace NewGraph
{

    // [CustomEditor(typeof(AssetObjectGraphModel))]
    // public class AssetObjectGraphModelEditor : GraphModelEditorBase
    // {
    //     [OnOpenAsset]
    //     public static bool OnOpenAsset(int instanceID, int line)
    //     {
    //         IGraphModelData baseGraphModel = EditorUtility.InstanceIDToObject(instanceID) as IGraphModelData;
    //         if (baseGraphModel != null)
    //         {
    //             baseGraphModel.CreateSerializedObject();
    //             OpenGraph(baseGraphModel);
    //             return true;
    //         }
    //         return false;
    //     }
    // }

}