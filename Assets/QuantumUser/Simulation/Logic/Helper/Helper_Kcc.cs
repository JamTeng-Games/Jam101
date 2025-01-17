using System;
using Photon.Deterministic;
using Quantum.Core;

namespace Quantum.Helper
{

    public enum KccMovementType
    {
        None,
        Free,   // 自由移动
        Tangent // 切线移动
    }

    public struct KccMovementData
    {
        public KccMovementType Type;
        public FPVector2 Correction; // 修正
        public FPVector2 Direction;  // 移动方向
        public FP MaxPenetration;    //最大穿透深度
    }

    public static unsafe class Helper_KCC
    {
        public static void SteerAndMove(Frame f, EntityRef entity, in KccMovementData movementData, KccSettings settings)
        {
            if (!f.Unsafe.TryGetPointer(entity, out KccComp* kcc))
                return;

            if (!f.Unsafe.TryGetPointer(entity, out Transform2D* transform))
                return;

            // 要移动
            if (movementData.Type != KccMovementType.None)
            {
                kcc->Velocity += kcc->Acceleration * movementData.Direction;
                if (kcc->Velocity.SqrMagnitude > kcc->MaxSpeed * kcc->MaxSpeed)
                {
                    kcc->Velocity = kcc->Velocity.Normalized * kcc->MaxSpeed;
                }
                // transform->Rotation = FPVector2.RadiansSigned(FPVector2.Up, movementData.Direction);// FPMath.Atan2(kcc->Velocity.Y, kcc->Velocity.X);
            }
            // 减速
            else
            {
                kcc->Velocity = FPVector2.MoveTowards(kcc->Velocity, FPVector2.Zero, settings.Brake);
            }

            // 当最大穿透深度超过允许的穿透深度时：
            FPVector2 correction = FPVector2.Zero;
            if (movementData.MaxPenetration > settings.AllowedPenetration)
            {
                // 如果穿透过大（超过允许穿透的两倍），立即应用全额修正
                if (movementData.MaxPenetration > settings.AllowedPenetration * 2)
                {
                    correction = movementData.Correction;
                }
                // 否则，根据时间增量和修正速度来部分应用修正。
                else
                {
                    correction = movementData.Correction * settings.CorrectionSpeed;
                }
            }

            // 请求旋转
            var velocity = kcc->Velocity;
            if (velocity != FPVector2.Zero)
            {
                FP rotation = FPMath.Atan2(velocity.Y, velocity.X) - FP.Pi / 2;
                // FP rotation = FPMath.Atan2(velocity.Y, velocity.X);
                Helper_Move.ReqRotateTo(f, entity, rotation);
            }
            // 请求移动
            Helper_Move.ReqMove(f, entity, velocity, correction);

#if UNITY_EDITOR
            if (settings.Debug)
            {
                Draw.Circle(transform->Position, settings.Radius, ColorRGBA.Blue);
                Draw.Ray(transform->Position, transform->Forward * settings.Radius, ColorRGBA.Red);
            }
#endif
        }

        public static KccMovementData ComputeRawMovement(Frame f, EntityRef entity, FPVector2 direction, KccSettings settings)
        {
            if (!f.Exists(entity))
                return default;

            if (!f.Unsafe.TryGetPointer(entity, out KccComp* kcc))
                return default;

            if (!f.Unsafe.TryGetPointer(entity, out Transform2D* transform))
                return default;

            KccMovementData movementPack = default;
            movementPack.Type = direction != default ? KccMovementType.Free : KccMovementType.None;
            movementPack.Direction = direction;

            // check hit statics
            Shape2D shape = Shape2D.CreateCircle(settings.Radius);
            var layer = f.Layers.GetLayerMask("Static");
            var hits = f.Physics2D.OverlapShape(transform->Position, FP._0, shape, layer,
                                                options: QueryOptions.HitStatics | QueryOptions.ComputeDetailedInfo);
            int count = Math.Min(settings.MaxContacts, hits.Count);
            if (hits.Count > 0)
            {
                Boolean initialized = false;
                hits.Sort(transform->Position);
                for (int i = 0; i < hits.Count && count > 0; i++)
                {
                    // ignore triggers
                    if (hits[i].IsTrigger)
                        continue;

                    // ignore self
                    if (hits[i].Entity == entity)
                        continue;

                    FPVector2 contactPoint = hits[i].Point;
                    FPVector2 contactToCenter = transform->Position - contactPoint;
                    FP localDiff = contactToCenter.Magnitude - settings.Radius;
#if UNITY_EDITOR
                    if (settings.Debug)
                    {
                        Draw.Circle(contactPoint, FP._0_10, ColorRGBA.Red);
                    }
#endif
                    var localNormal = contactToCenter.Normalized;
                    count--;

                    // define movement type
                    if (!initialized)
                    {
                        initialized = true;
                        if (direction != default)
                        {
                            var angle = FPVector2.RadiansSkipNormalize(direction.Normalized, localNormal);
                            if (angle >= FP.Rad_90)
                            {
                                var d = FPVector2.Dot(direction, localNormal);
                                var tangentVelocity = direction - localNormal * d;
                                if (tangentVelocity.SqrMagnitude > FP.EN4)
                                {
                                    movementPack.Direction = tangentVelocity.Normalized;
                                    movementPack.Type = KccMovementType.Tangent;
                                }
                                else
                                {
                                    movementPack.Direction = default;
                                    movementPack.Type = KccMovementType.None;
                                }
                            }
                            movementPack.MaxPenetration = FPMath.Abs(localDiff);
                        }
                    }
                    // any real contact contributes to correction and average normal
                    var localCorrection = localNormal * -localDiff;
                    movementPack.Correction += localCorrection;
                }
            }
            return movementPack;
        }
    }

}