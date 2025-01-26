using Photon.Deterministic;
using Quantum.Graph.Skill;
using Quantum.Physics2D;

namespace Quantum.Helper
{

    public unsafe class BulletCmd_Arrow : BulletCmd
    {
        public override void OnCreate(Frame f, EntityRef bulletEntity, BulletComp* bulletComp)
        {
            Transform2D* transBullet = f.Unsafe.GetPointer<Transform2D>(bulletEntity);
            Helper_Move.ForceRotate(f, bulletComp->Caster, transBullet->Rotation - FP.Pi / 2);
        }

        public override void OnHit(Frame f, EntityRef bulletEntity, BulletComp* bulletComp, EntityRef target, in Hit hitInfo)
        {
            if (hitInfo.IsStatic)
                return;

            // Helper_Damage.DoDamage(f, filter.Entity, filter.Entity, EDamageInfoType.DirectDamage,
            //                        new Damage() { bullet = 10, aoe = 0, }, FP._1, FP._0, FP._0_20, 0);
            
            Log.Debug($"BulletCmd_Arrow OnHit");
            Transform2D trans = f.Get<Transform2D>(bulletEntity);
            Helper_Damage.DoDamage(f, new DamageInfo()
            {
                source = bulletComp->Caster,
                target = target,
                damageType = EDamageInfoType.DirectDamage,
                damage = new Damage() { bullet = 10, },
                hitRate = 1,
                criticalRate = 0,
                angle = trans.Rotation,
            });
            f.Events.OnHit(target);
        }
    }

}