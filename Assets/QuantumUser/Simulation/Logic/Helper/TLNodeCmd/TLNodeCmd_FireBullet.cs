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
                fireBulletInfo.firePos = trans.Position;
                fireBulletInfo.fireAngle = trans.Rotation;
                Helper_Bullet.Fire(f, fireBulletInfo);
            }
        }
    }

}