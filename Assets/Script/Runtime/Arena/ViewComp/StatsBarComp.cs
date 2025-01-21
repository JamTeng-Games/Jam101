using Quantum;
using Quantum.Graph.Skill;
using Quantum.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Arena
{

    public class StatsBarComp : JamEntityViewComp
    {
        [SerializeField] private Transform _uiRoot;
        [SerializeField] private Slider _hp;
        [SerializeField] private TextMeshProUGUI _hpTxt;
        [SerializeField] private Transform _bulletRoot;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Slider _superSkill;

        private Camera _camera;
        private int _maxHp;
        private int _attackClipNum;

        public override void OnActivate(Frame frame)
        {
            _camera = Camera.main;
            Frame f = VerifiedFrame;
            Helper_Attrib.TryGetAttribValue(f, EntityRef, AttributeType.MaxHp, out _maxHp);
            // Helper_Attrib.TryGetAttribValue(f, EntityRef, AttributeType.AttackClipMaxNum, out _attackClipNum);
        }

        public override void OnUpdateView()
        {
            Frame f = VerifiedFrame;

            // Hp
            int hp = Helper_Stats.GetHp(f, EntityRef);
            _hp.value = hp / (float)_maxHp;
            _hpTxt.text = $"{hp}/{_maxHp}";
        }

        public override void OnLateUpdateView()
        {
            _uiRoot.LookAt(_uiRoot.position + _camera.transform.forward);
        }
    }

}