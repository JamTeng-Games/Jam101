using Jam.Cfg;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Jam.Editor_
{

    public class NewBulletData
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public BulletTemplateSO bulletData;

        private BulletWindow _window;

        public NewBulletData(BulletWindow window)
        {
            _window = window;
            bulletData = ScriptableObject.CreateInstance<BulletTemplateSO>();
            bulletData.name = "None";
        }

        public void Dispose()
        {
            Object.DestroyImmediate(bulletData);
            bulletData = null;
        }

        [Button("创建新子弹", ButtonSizes.Large)]
        private void Create()
        {
            AssetDatabase.CreateAsset(bulletData, $"Assets/Res_Nopack/SO/Bullet/{bulletData.name}.asset");
            AssetDatabase.SaveAssets();
            _window.MarkDirty();

            bulletData = ScriptableObject.CreateInstance<BulletTemplateSO>();
            bulletData.name = "None";
        }
    }

}