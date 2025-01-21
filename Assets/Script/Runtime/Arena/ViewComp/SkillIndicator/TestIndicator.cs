using Photon.Deterministic;
using Quantum.Graph.Skill;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jam.Arena
{

    public class TestIndicator : MonoBehaviour
    {
        [SerializeField] private SkillIndicatorControl _control;

        [Button]
        public void ShowArc()
        {
            _control.ShowIndicator(new SkillIndicatorInfo() { arc = 90, radius = 2f, indicatorType = IndicatorType.Arc, });
        }
    }

}