using Jam.Cfg;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_
{

    public class NewBuffData
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public BuffTemplateSO buffData;

        private BuffWindow _window;

        public NewBuffData(BuffWindow window)
        {
            _window = window;
            buffData = ScriptableObject.CreateInstance<BuffTemplateSO>();
            buffData.name = "None";
        }

        public void Dispose()
        {
            Object.DestroyImmediate(buffData);
            buffData = null;
        }

        [Button("创建新Buff", ButtonSizes.Large)]
        private void Create()
        {
            AssetDatabase.CreateAsset(buffData, $"Assets/Res_Nopack/SO/Buff/{buffData.name}.asset");
            AssetDatabase.SaveAssets();
            _window.MarkDirty();

            buffData = ScriptableObject.CreateInstance<BuffTemplateSO>();
            buffData.name = "None";
        }
    }

}