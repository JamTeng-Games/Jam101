using Jam.Cfg;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_
{

    public class NewAoeData
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public AoeTemplateSO aoeData;

        private AoeWindow _window;

        public NewAoeData(AoeWindow window)
        {
            _window = window;
            aoeData = ScriptableObject.CreateInstance<AoeTemplateSO>();
            aoeData.name = "None";
        }

        public void Dispose()
        {
            Object.DestroyImmediate(aoeData);
            aoeData = null;
        }

        [Button("创建新AOE", ButtonSizes.Large)]
        private void Create()
        {
            AssetDatabase.CreateAsset(aoeData, $"Assets/Res_Nopack/SO/Aoe/{aoeData.name}.asset");
            AssetDatabase.SaveAssets();
            _window.MarkDirty();

            aoeData = ScriptableObject.CreateInstance<AoeTemplateSO>();
            aoeData.name = "None";
        }
    }

}