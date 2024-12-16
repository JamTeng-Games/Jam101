using System;
using Jam.Runtime;
using Quantum;
using UnityEngine;

namespace Jam.Arena
{

    public class EntityViewSpawner : JamEntityViewComp
    {
        private bool _isLocalPlayer;
        private int _assetHandleId = 0;

        public override void OnActivate(Frame frame)
        {
            var link = frame.Get<PlayerComp>(EntityRef);
            _isLocalPlayer = Game.PlayerIsLocal(link.PlayerRef);

            if (_isLocalPlayer)
            {
                ViewContext.followCamera.Follow = transform;
                Log.Info("EntitySpawnView");
            }

            // var entityAsset = Resources.Load<GameObject>("Knight_HeroView");
            // var entityGo = Instantiate(entityAsset, transform);
            // entityGo.transform.localPosition = Vector3.zero;
            // entityGo.transform.localRotation = Quaternion.identity;
            //
            // // Attach ViewComps
            // if (EntityView is JamEntityView ev)
            // {
            //     var quantumMono = entityGo.GetComponentsInChildren<JamEntityViewComp>();
            //     foreach (var c in quantumMono)
            //     {
            //         ev.AddViewComp(c);
            //     }
            // }

            G.Asset.Load("Assets/Res/Prefab/Hero/Knight_HeroView.prefab", typeof(GameObject), (obj) =>
            {
                var prefab = (GameObject)obj.Asset;
                var hero = Instantiate(prefab, transform);
                hero.transform.localPosition = Vector3.zero;
                hero.transform.localRotation = Quaternion.identity;
                _assetHandleId = obj.Id;

                // Attach ViewComps
                if (EntityView is JamEntityView ev)
                {
                    var quantumMono = hero.GetComponentsInChildren<JamEntityViewComp>();
                    foreach (var c in quantumMono)
                    {
                        ev.AddViewComp(c);
                    }
                }
            }, null);
        }

        public override void OnDeactivate()
        {
            // if (_assetHandleId != 0)
            //     G.Asset.Unload(_assetHandleId);
        }

        public override void OnUpdateView()
        {
        }
    }

}