using Jam.Core;

namespace Jam.Runtime.Constant
{

    public static class AssetPath
    {
        public static string Cfg(string assetName, bool fromBytes) =>
            Utils.Text.Format("Assets/Res/Config/Gen/{0}.{1}", assetName, fromBytes ? "bytes" : "json");

        public static string UIPanel(string assetName) => Utils.Text.Format("Assets/Res/UI/Panel/{0}.prefab", assetName);

        public static string UIWidget(string assetName) => Utils.Text.Format("Assets/Res/UI/Widget/{0}.prefab", assetName);

        public static string Font(string assetName) => Utils.Text.Format("Assets/Res/Fonts/{0}.ttf", assetName);

        public static string Scene(string assetName) => Utils.Text.Format("Assets/Res/Scenes/{0}.unity", assetName);

        public static string Music(string assetName) => Utils.Text.Format("Assets/Res/Music/{0}.mp3", assetName);

        public static string Sound(string assetName) => Utils.Text.Format("Assets/Res/Sounds/{0}.wav", assetName);

        public static string Entity(string assetName) => Utils.Text.Format("Assets/Res/Entities/{0}.prefab", assetName);

        public static string UISound(string assetName) => Utils.Text.Format("Assets/Res/UI/UISounds/{0}.wav", assetName);

        public static string HeroIcon(string assetName) => Utils.Text.Format("Assets/Res/UI/Sprite/Hero/{0}.png", assetName);
        public static string HeroPrefab(string assetName) => Utils.Text.Format("Assets/Res/Prefab/Hero/{0}.prefab", assetName);

        public static string ItemIcon(string assetName) => Utils.Text.Format("Assets/Res/UI/Sprite/Item/{0}.png", assetName);
    }

}