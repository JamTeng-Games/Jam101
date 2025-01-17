using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Collections;
using Quantum.Physics2D;

namespace Quantum.Helper
{

    public static unsafe partial class Helper_Bullet
    {
        public static void Fire(Frame f, FireBulletInfo fireBulletInfo)
        {
            if (f.Unsafe.TryGetPointerSingleton<SBulletFireInfoComp>(out var bulletFireComp))
            {
                // TODO: 在这里选择目标，调整子弹的角度
                var infos = f.ResolveList(bulletFireComp->FireBulletInfos);
                infos.Add(fireBulletInfo);
            }

            // var playerEntity = f.Create(runtimePlayer.PlayerAvatar);
            // f.AddOrGet<PlayerComp>(playerEntity, out var playerComp);
            // playerComp->PlayerRef = playerRef;
            // f.Signals.OnPlayerSpawned(playerEntity, playerRef);
        }

        public static bool CanHit(in QList<BulletHitRecord> hitRecords, EntityRef target)
        {
            for (int i = 0; i < hitRecords.Count; i++)
            {
                if (hitRecords[i].target == target)
                {
                    return false;
                }
            }
            return true;
        }

        #region Bullet Cmds

        // Bullet id -> BulletCmd
        private static Dictionary<int, BulletCmd> _bulletCmds;

        public static void OnCreate(Frame f, EntityRef entity, BulletComp* bulletComp)
        {
            if (_bulletCmds.TryGetValue(bulletComp->Model.type, out var bulletCmd))
            {
                bulletCmd.OnCreate(f, entity, bulletComp);
            }
        }

        public static void OnRemove(Frame f, EntityRef entity, BulletComp* bulletComp)
        {
            if (_bulletCmds.TryGetValue(bulletComp->Model.type, out var bulletCmd))
            {
                bulletCmd.OnRemove(f, entity, bulletComp);
            }
        }

        public static void OnTick(Frame f, EntityRef entity, BulletComp* bulletComp)
        {
            if (_bulletCmds.TryGetValue(bulletComp->Model.type, out var bulletCmd))
            {
                bulletCmd.OnTick(f, entity, bulletComp);
            }
        }

        public static (FPVector2, FP) OnTween(Frame f, EntityRef entity, BulletComp* bulletComp, EntityRef target)
        {
            if (_bulletCmds.TryGetValue(bulletComp->Model.type, out var bulletCmd))
            {
                return bulletCmd.OnTween(f, entity, bulletComp, target);
            }
            return default;
        }

        public static void OnHit(Frame f, EntityRef entity, BulletComp* bulletComp, EntityRef target, in Hit hitInfo)
        {
            Log.Info($"bulletComp->OnHit");
            if (_bulletCmds.TryGetValue(bulletComp->Model.type, out var bulletCmd))
            {
                bulletCmd.OnHit(f, entity, bulletComp, target, hitInfo);
            }
        }

        #endregion
    }

}