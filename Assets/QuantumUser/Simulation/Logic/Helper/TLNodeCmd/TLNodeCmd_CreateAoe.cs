using Photon.Deterministic;

namespace Quantum.Helper
{

    public static unsafe partial class Helper_TLNode
    {
        private class TLNodeCmd_CreateAoe : ITLNodeCmd
        {
            public void Execute(Frame f, TimelineObj tlObj, TLNode node)
            {
                Log.Debug("TLNodeCmd_CreateAoe.Execute 1");

                var createAoeInfo = node.CreateAoe->createAoeInfo;
                createAoeInfo.caster = tlObj.caster;

                var caster = tlObj.caster;
                if (!f.TryGet<Transform2D>(caster, out var trans))
                    return;
                Log.Debug("TLNodeCmd_CreateAoe.Execute 2");

                FP angleRad = trans.Rotation;
                if (f.TryGet<InputComp>(caster, out var inputComp))
                {
                    angleRad = Helper_Math.DirectionToAngleRad(inputComp.Input.AimDirection) - FP.Pi / 2;
                }

                // 目前先按英雄方向发射子弹
                createAoeInfo.position = trans.Position;
                createAoeInfo.angle = angleRad;
                Helper_Aoe.Create(f, createAoeInfo);
                // Helper_Bullet.Fire(f, createAoeInfo);
            }
        }
    }

}