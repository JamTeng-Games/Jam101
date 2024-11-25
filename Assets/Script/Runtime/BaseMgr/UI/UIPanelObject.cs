using cfg;
using Jam.Core;
using Jam.Runtime.ObjectPool;
using UnityEngine;

namespace Jam.Runtime.UI_
{

    public class UIPanelObject : ObjectBase
    {
        // Prefab
        private int _assetHandleId = 0;

        public static UIPanelObject Create(string assetPath, int assetHandleId, object panel)
        {
            UIPanelObject obj = ReferencePool.Get<UIPanelObject>();
            obj.Initialize(assetPath, panel);
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
            GameObject.Destroy(((UIPanel)Target).gameObject);
        }
    }

}