using System;
using Jam.Runtime;
using Jam.Runtime.Constant;
using Quantum;
using UnityEngine;

namespace Jam.Arena
{

    public class BulletViewSpawner : JamEntityViewComp
    {
        public override void OnActivate(Frame frame)
        {
            var bullet = frame.Get<BulletComp>(EntityRef);
            int bulletId = bullet.Model.type;

            // TODO: Find bullet prefab
            var asset = Resources.Load<GameObject>("BulletPrefab/test_bullet");
            InstantiateModel(asset);
        }

        public override void OnDeactivate()
        {
        }

        private void InstantiateModel(GameObject asset)
        {
            var hero = Instantiate(asset, transform);
            hero.transform.localPosition = Vector3.zero;
            hero.transform.localRotation = Quaternion.identity;
            // _assetHandleId = obj.Id;

            // Attach ViewComps
            if (EntityView is JamEntityView ev)
            {
                var quantumMono = hero.GetComponentsInChildren<JamEntityViewComp>();
                foreach (var c in quantumMono)
                {
                    ev.AddViewComp(c);
                }
            }
        }
    }

}