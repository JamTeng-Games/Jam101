using System;
using Jam.Runtime;
using Jam.Runtime.Constant;
using Quantum;
using UnityEngine;

namespace Jam.Arena
{

    public class EntityViewSpawner : JamEntityViewComp
    {
        public static EntityRef LocalPlayerEntityRef;
        public static int localCount = 0;

        private bool _isLocalPlayer;
        // private int _assetHandleId = 0;

        public override void OnActivate(Frame frame)
        {
            Log.Info("EntityViewSpawner OnActivate");

            string loadPath = string.Empty;
            if (frame.TryGet<PlayerComp>(EntityRef, out var playerComp))
            {
                _isLocalPlayer = Game.PlayerIsLocal(playerComp.PlayerRef);
                if (_isLocalPlayer)
                {
                    localCount++;
                }
                if (_isLocalPlayer && ViewContext.followCamera.Follow == null)
                {
                    ViewContext.followCamera.Follow = transform;
                    LocalPlayerEntityRef = EntityRef;
                }

                int heroId = frame.GetPlayerData(playerComp.PlayerRef).heroData.hero;
                var heroCfg = Quantum.Cfg_.Cfg.Tb.TbHero[heroId];
                loadPath = $"HeroPrefab/{heroCfg.ModelArena}";

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
            else if (frame.TryGet<BulletComp>(EntityRef, out var bulletComp))
            {
                int bulletId = bulletComp.Model.type;
                // TODO: Find bullet prefab
                loadPath = "BulletPrefab/test_bullet";
            }
            else if (frame.TryGet<AoeComp>(EntityRef, out var aoeComp))
            {
                // TODO: Find aoe prefab
                int aoeId = aoeComp.Model.type;
                loadPath = "AoePrefab/test_aoe";
            }

            if (string.IsNullOrEmpty(loadPath))
                return;
            GameObject entityAsset = Resources.Load<GameObject>(loadPath);
            InstantiateModel(entityAsset, _isLocalPlayer);
        }

        public override void OnDeactivate()
        {
            // if (_assetHandleId != 0)
            //     G.Asset.Unload(_assetHandleId);
        }

        public override void OnUpdateView()
        {
        }

        private void InstantiateModel(GameObject asset, bool isLocalPlayer)
        {
            var go = Instantiate(asset, transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            // _assetHandleId = obj.Id;

            if (isLocalPlayer && localCount == 1)
            {
                AddLocalPlayerComps(go);
            }

            // Attach ViewComps
            if (EntityView is JamEntityView ev)
            {
                var quantumMono = go.GetComponentsInChildren<JamEntityViewComp>();
                foreach (var c in quantumMono)
                {
                    ev.AddViewComp(c);
                }
            }
        }

        private void AddLocalPlayerComps(GameObject go)
        {
            if (EntityView is JamEntityView ev)
            {
                go.AddComponent<SkillIndicatorComp>();
            }
        }
    }

}