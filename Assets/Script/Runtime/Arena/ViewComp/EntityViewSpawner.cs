using System;
using Jam.Runtime;
using Jam.Runtime.Constant;
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
            Log.Info("EntityViewSpawner OnActivate");
            var link = frame.Get<PlayerComp>(EntityRef);
            _isLocalPlayer = Game.PlayerIsLocal(link.PlayerRef);

            if (_isLocalPlayer && ViewContext.followCamera.Follow == null)
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

            int heroId = frame.GetPlayerData(link.PlayerRef).heroData.hero;
            var heroCfg = Quantum.Cfg_.Cfg.Tb.TbHero[heroId];

            var entityAsset = Resources.Load<GameObject>($"HeroPrefab/{heroCfg.ModelArena}");
            InstantiateModel(entityAsset);

            // G.Asset.Load(AssetPath.HeroPrefab(heroCfg.ModelArena), typeof(GameObject), (obj) =>
            // {
            //     InstantiateHeroModel((GameObject)obj.Asset);
            //     // var prefab = (GameObject)obj.Asset;
            //     // var hero = Instantiate(prefab, transform);
            //     // hero.transform.localPosition = Vector3.zero;
            //     // hero.transform.localRotation = Quaternion.identity;
            //     // _assetHandleId = obj.Id;
            //     //
            //     // // Attach ViewComps
            //     // if (EntityView is JamEntityView ev)
            //     // {
            //     //     var quantumMono = hero.GetComponentsInChildren<JamEntityViewComp>();
            //     //     foreach (var c in quantumMono)
            //     //     {
            //     //         ev.AddViewComp(c);
            //     //     }
            //     // }
            // }, null);
        }

        public override void OnDeactivate()
        {
            // if (_assetHandleId != 0)
            //     G.Asset.Unload(_assetHandleId);
        }

        public override void OnUpdateView()
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