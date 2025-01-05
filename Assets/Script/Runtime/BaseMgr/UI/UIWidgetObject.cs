using Jam.Core;
using Jam.Runtime.ObjectPool;
using UnityEngine;

namespace Jam.Runtime.UI_
{

    public sealed class UIWidgetObject : ObjectBase
    {
        // Prefab
        private int _assetHandleId = 0;

        public static UIWidgetObject Create(string assetPath, int assetHandleId, object widget)
        {
            UIWidgetObject obj = ReferencePool.Get<UIWidgetObject>();
            obj.Initialize(assetPath, widget);
            obj._assetHandleId = assetHandleId;
            return obj;
        }

        public override void Clean()
        {
            base.Clean();
            _assetHandleId = 0;
        }

        public override void Release(bool isShutdown)
        {
            G.Asset.Unload(_assetHandleId);
            GameObject.Destroy(((UIWidget)Target).gameObject);
        }
    }

}