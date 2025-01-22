using System;
using Jam.Core;
using Photon.Deterministic;
using Quantum;
using Quantum.Graph.Skill;
using Quantum.Helper;
using UnityEngine;

namespace Jam.Arena
{

    [Serializable]
    public struct SkillIndicatorInfo
    {
        public IndicatorType indicatorType;
        public float maxRange;
        public float radius;
        public int arc;
        public float width;
        // patch
        public float angle;
        public Vector2 posOffset;
    }

    public class SkillIndicatorComp : JamEntityViewComp
    {
        private bool IsPrepareAttack => TestInputPolling.IsPrepareAttack;
        private bool IsPrepareSkill => TestInputPolling.IsPrepareSkill;
        private bool IsPrepareSuperSkill => TestInputPolling.IsPrepareSuperSkill;
        private FPVector2 MouseScreenPos => TestInputPolling.MouseScreenPos;
        private bool IsPreparing => IsPrepareAttack || IsPrepareSkill || IsPrepareSuperSkill;

        private Camera _camera;
        private int _skillTypeShowing = 0;
        private SkillIndicatorControl _control;

        public override void OnActivate(Frame frame)
        {
            _camera = Camera.main;
            if (_camera == null)
            {
                Log.Error("Main camera is null");
            }
            if (_control == null)
            {
                var prefab = Resources.Load<GameObject>("AttackPreview/IndicatorControl");
                var go = Instantiate(prefab, transform);
                var newPos = go.transform.position;
                newPos.y = 0.1f;
                go.transform.position = newPos;
                _control = go.GetComponent<SkillIndicatorControl>();
            }
        }

        public override void OnUpdateView()
        {
            if (TryGetPreparingSkillObj(out SkillObj skillObj))
            {
                SkillModel model = skillObj.model;
                SkillIndicatorInfo info = new SkillIndicatorInfo()
                {
                    indicatorType = (IndicatorType)model.indicatorType,
                    maxRange = (model.indicatorMaxRange / FP._1000).AsFloat,
                    radius = (model.indicatorRadius / FP._1000).AsFloat,
                    arc = model.indicatorArc,
                    width = (model.indicatorWidth / FP._1000).AsFloat,
                };

                if (_skillTypeShowing != skillObj.model.type)
                {
                    _skillTypeShowing = skillObj.model.type;
                    PatchExtraInfo(ref info);
                    _control.ShowIndicator(info);
                }
                else
                {
                    PatchExtraInfo(ref info);
                    _control.UpdateIndicator(info);
                }
            }
            else
            {
                _skillTypeShowing = 0;
                _control.HideAll();
            }
        }

        private void PatchExtraInfo(ref SkillIndicatorInfo info)
        {
            InputComp input = PredictedFrame.Get<InputComp>(EntityRef);

            var trans2d = PredictedFrame.Get<Transform2D>(EntityRef);

            FPVector2 aimDir = input.Input.AimDirection;
            FP aimLen = input.Input.AimLength;

            // DebugExtension.DebugArrow(new Vector3(pos.x, 0, pos.y), new Vector3(aimDir.X.AsFloat, 0, aimDir.Y.AsFloat));

            float angle = -Vector2.SignedAngle(Vector2.up, aimDir.ToUnityVector2()) - (-trans2d.Rotation * FP.Rad2Deg).AsFloat;
            info.angle = angle;

            info.posOffset = Helper_Math.AngleRadToDirectionF(-angle * Mathf.Deg2Rad + Mathf.PI / 2f) * aimLen.AsFloat;
        }

        private bool TryGetPreparingSkillObj(out SkillObj skillObj)
        {
            if (IsPrepareAttack)
                return Helper_Skill.TryGetSkillObjByType(PredictedFrame, EntityRef, SkillType.Attack, out skillObj);

            if (IsPrepareSkill)
                return Helper_Skill.TryGetSkillObjByType(PredictedFrame, EntityRef, SkillType.HeroSkill, out skillObj);

            if (IsPrepareSuperSkill)
                return Helper_Skill.TryGetSkillObjByType(PredictedFrame, EntityRef, SkillType.SuperSkill, out skillObj);

            skillObj = default;
            return false;
        }
    }

}