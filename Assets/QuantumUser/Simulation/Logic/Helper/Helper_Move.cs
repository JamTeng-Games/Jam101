using System.IO;
using Photon.Deterministic;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public unsafe class Helper_Move
    {
        public static void Move(Frame f, EntityRef entity, FPVector2 moveVector)
        {
            if (f.Unsafe.TryGetPointer<Transform2D>(entity, out var transComp))
            {
                moveVector /= 300;
                if (Helper_Attrib.TryGetAttribValue(f, entity, AttributeType.Speed, out var speed))
                {
                    Log.Debug($"Speed {speed}");
                    moveVector *= (FP)speed;
                }
                transComp->Position += moveVector;

                // TODO: 旋转单独一个组件
                if (moveVector != FPVector2.Zero)
                {
                    transComp->Rotation = FPMath.Atan2(moveVector.Y, moveVector.X) - FP.Pi / 2;
                }
                f.Events.OnMove(entity, moveVector);
            }
        }

        public static void ReqRotateTo(Frame f, EntityRef entity, FP degree)
        {
        }

        // move表示距离和方向
        public static void ReqMove(Frame f, EntityRef entity, FPVector2 moveVector)
        {
            if (f.Unsafe.TryGetPointer<MoveComp>(entity, out var moveComp))
            {
                moveComp->Vector = moveVector;
            }
        }

        // Force Move --- move表示距离和方向
        public static void ReqForceMove(Frame f, EntityRef entity, MovePreorder move)
        {
            if (f.Unsafe.TryGetPointer<ForceMoveComp>(entity, out var forceMoveComp))
            {
                var forceList = f.ResolveList(forceMoveComp->MovePreorder);
                forceList.Add(move);
            }
        }

        public static bool TryConsumeForceMove(Frame f, EntityRef entity, out FPVector2 totalMove)
        {
            totalMove = FPVector2.Zero;
            if (!f.Unsafe.TryGetPointer<ForceMoveComp>(entity, out var forceMoveComp))
                return false;

            var forceList = f.ResolveList(forceMoveComp->MovePreorder);
            for (int i = forceList.Count - 1; i >= 0; i--)
            {
                var force = forceList[i];
                // 减少剩余时间
                force.remainFrame--;
                // 累加Move
                totalMove += force.totalFrame switch
                {
                    <= 0 => forceList[i].vector,
                    _    => forceList[i].vector / force.totalFrame
                };
                // 判断是否要剔除
                if (force.remainFrame <= 0)
                {
                    forceList.RemoveAt(i);
                }
                // 不剔除的写回
                else
                {
                    forceList[i] = force;
                }
            }
            return true;
        }
    }

}