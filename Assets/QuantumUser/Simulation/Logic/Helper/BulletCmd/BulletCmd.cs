using Photon.Deterministic;
using Quantum.Graph.Skill;
using Quantum.Physics2D;

namespace Quantum.Helper
{

    // 执行Buff Command
    public unsafe class BulletCmd
    {
        public virtual void OnCreate(Frame f, EntityRef bulletEntity, BulletComp* bulletComp)
        {
        }

        public virtual void OnRemove(Frame f, EntityRef bulletEntity, BulletComp* bulletComp)
        {
        }

        public virtual void OnTick(Frame f, EntityRef bulletEntity, BulletComp* bulletComp)
        {
        }

        public virtual (FPVector2, FP) OnTween(Frame f, EntityRef bulletEntity, BulletComp* bulletComp, EntityRef target)
        {
            TweenType tweenType = (TweenType)bulletComp->Model.tweenType;
            Transform2D trans = f.Get<Transform2D>(bulletEntity);
            FP speed = bulletComp->Speed;
            FPVector2 offset = trans.Forward * speed;
            FP angle = trans.Rotation;
            return (offset, angle);
        }

        public virtual void OnHit(Frame f, EntityRef bulletEntity, BulletComp* bulletComp, EntityRef target, in Hit hitInfo)
        {
        }
    }

}