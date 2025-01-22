using DTT.AreaOfEffectRegions;
using MoreMountains.Feedbacks;
using Photon.Deterministic;
using Quantum;
using Quantum.Graph.Skill;
using UnityEngine;

namespace Jam.Arena
{

    public class SkillIndicatorControl : MonoBehaviour
    {
        [SerializeField] private ArcRegion _arc;
        [SerializeField] private LineRegion _line;
        [SerializeField] private CircleRegion _circle;
        [SerializeField] private CircleRegion _moveCircle;
        [SerializeField] private ScatterLineRegion _scatter;

        [SerializeField] private CircleRegion _rangeCircle;

        public SkillIndicatorInfo info;

        public void ShowIndicator(SkillIndicatorInfo info)
        {
            _arc.gameObject.SetActive(info.indicatorType == IndicatorType.Arc);
            _circle.gameObject.SetActive(info.indicatorType == IndicatorType.Circle);
            _line.gameObject.SetActive(info.indicatorType == IndicatorType.Line);
            _scatter.gameObject.SetActive(info.indicatorType == IndicatorType.Scatter);
            _moveCircle.gameObject.SetActive(info.indicatorType == IndicatorType.MoveCircle);
            // _rangeCircle.gameObject.SetActive(info.indicatorType == IndicatorType.MoveCircle);
            UpdateIndicator(info);
        }

        public void UpdateIndicator(SkillIndicatorInfo info)
        {
            if (info.indicatorType == IndicatorType.Arc)
            {
                _arc.Arc = info.arc;
                _arc.Angle = info.angle;
                _arc.Radius = info.radius;
            }
            else if (info.indicatorType == IndicatorType.Circle)
            {
                _circle.Radius = info.radius;
            }
            else if (info.indicatorType == IndicatorType.MoveCircle)
            {
                if (info.posOffset.magnitude > info.maxRange)
                    info.posOffset = info.posOffset.normalized * info.maxRange;
                _moveCircle.Offset = info.posOffset;
                _moveCircle.Radius = info.radius;
                // _rangeCircle.Radius = info.maxRange;
            }
            else if (info.indicatorType == IndicatorType.Line)
            {
                _line.Angle = info.angle;
                _line.Length = info.radius;
            }
            else if (info.indicatorType == IndicatorType.Scatter)
            {
                _scatter.Arc = info.angle;
                _scatter.Length = info.radius;
                _scatter.Width = info.width;
            }
        }

        public void HideAll()
        {
            _arc.gameObject.SetActive(false);
            _line.gameObject.SetActive(false);
            _circle.gameObject.SetActive(false);
            _moveCircle.gameObject.SetActive(false);
            _scatter.gameObject.SetActive(false);
            // _rangeCircle.gameObject.SetActive(false);
        }
    }

}