using Photon.Deterministic;

namespace Quantum.Helper
{

    public static unsafe partial class Helper_TLNode
    {
        private class TLNodeCmd_FireBullet : ITLNodeCmd
        {
            public void Execute(Frame f, TimelineObj tlObj, TLNode node)
            {
                Log.Debug("TLNodeCmd_FireBullet.Execute 1");

                var fireBulletInfo = node.FireBullet->fireBulletInfo;
                fireBulletInfo.caster = tlObj.caster;

                var caster = tlObj.caster;
                if (!f.TryGet<Transform2D>(caster, out var trans))
                    return;
                Log.Debug("TLNodeCmd_FireBullet.Execute 2");

                // 目前先按英雄方向发射子弹
                FP angleRad = trans.Rotation;
                if (f.TryGet<InputComp>(caster, out var inputComp))
                {
                    angleRad = Helper_Math.DirectionToAngleRad(inputComp.Input.AimDirection) - FP.Pi / 2;
                }

                fireBulletInfo.firePos = trans.Position;
                fireBulletInfo.fireAngle = angleRad;
                Helper_Bullet.Fire(f, fireBulletInfo);
            }
        }
    }

}