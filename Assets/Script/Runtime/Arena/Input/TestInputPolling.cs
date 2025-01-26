using Jam.Core;
using Jam.Runtime.Input_;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam.Arena
{

    public class TestInputPolling : MonoBehaviour
    {
        private PlayerInputMap _inputMap;

        private Quantum.Input _accumulatedInput;
        private bool _resetAccumulatedInput;
        private int _lastAccumulateFrame;
        private Camera _camera;
        private InputMgr _inputMgr;
        private InputData _inputData;

        public static bool IsPrepareAttack { get; private set; }
        public static bool IsPrepareSkill { get; private set; }
        public static bool IsPrepareSuperSkill { get; private set; }
        public static bool IsAttack { get; private set; }
        public static bool IsSkill { get; private set; }
        public static bool IsSuperSkill { get; private set; }

        public static FPVector2 MouseScreenPos { get; private set; }

        private void OnEnable()
        {
            _inputMap = new PlayerInputMap();
            _inputMap.Enable();
            _inputMap.Basic.MoveDir.ReadValue<Vector2>();
            _camera = Camera.main;
            _inputMgr = Jam.Runtime.G.Input;
            _inputData = _inputMgr.Data;

            QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
        }

        private void Update()
        {
            AccumulateInput();
        }

        private void AccumulateInput()
        {
            if (_lastAccumulateFrame == Time.frameCount)
                return;

            _lastAccumulateFrame = Time.frameCount;

            if (_resetAccumulatedInput)
            {
                _resetAccumulatedInput = false;
                _accumulatedInput = default;
            }

            ProcessStandaloneInput();
        }

        // private void ProcessStandaloneInput()
        // {
        //     // Move dir
        //     _accumulatedInput.MoveDirection = _inputMap.Basic.MoveDir.ReadValue<Vector2>().normalized.ToFPVector2();
        //     MouseScreenPos = Mouse.current.position.ReadValue().ToFPVector2();
        //
        //     // Prepare
        //     // PrepareAttack
        //     IsPrepareAttack = Mouse.current.leftButton.isPressed;
        //     if (IsPrepareAttack)
        //     {
        //         _accumulatedInput.AttackPrepare = true;
        //     }
        //
        //     // PrepareSkill
        //     IsPrepareSkill = Mouse.current.rightButton.isPressed;
        //     if (IsPrepareSkill)
        //     {
        //         _accumulatedInput.SkillPrepare = true;
        //     }
        //
        //     // PrepareSuperSkill
        //     IsPrepareSuperSkill = Keyboard.current.spaceKey.isPressed;
        //     if (IsPrepareSuperSkill)
        //     {
        //         _accumulatedInput.SuperSkillPrepare = true;
        //     }
        //
        //     // Do
        //     // Attack
        //     IsAttack = Mouse.current.leftButton.wasReleasedThisFrame;
        //     if (IsAttack)
        //     {
        //         _accumulatedInput.Attack = true;
        //     }
        //     // Skill
        //     IsSkill = Mouse.current.rightButton.wasReleasedThisFrame;
        //     if (IsSkill)
        //     {
        //         _accumulatedInput.Skill = true;
        //     }
        //     // SuperSkill
        //     IsSuperSkill = Keyboard.current.spaceKey.wasReleasedThisFrame;
        //     if (IsSuperSkill)
        //     {
        //         _accumulatedInput.SuperSkill = true;
        //     }
        //
        //     // Cancel
        //     if (Keyboard.current.escapeKey.wasPressedThisFrame)
        //     {
        //         _accumulatedInput.Cancel = true;
        //     }
        //
        //     //
        //     _accumulatedInput.AimDirection = GetDirectionToMouse();
        //     _accumulatedInput.AimLength = _accumulatedInput.AimDirection.Magnitude;
        // }
        
        private void ProcessStandaloneInput()
        {
            // Move dir
            _accumulatedInput.MoveDirection = _inputData.moveDir.ToFPVector2();
            JLog.Debug($"ProcessStandaloneInput {_accumulatedInput.MoveDirection}");

            // Prepare
            // PrepareAttack
            IsPrepareAttack = _inputData.attackPrepare;
            if (IsPrepareAttack)
            {
                _accumulatedInput.AttackPrepare = true;
            }

            // PrepareSkill
            IsPrepareSkill = _inputData.skillPrepare;
            if (IsPrepareSkill)
            {
                _accumulatedInput.SkillPrepare = true;
            }

            // PrepareSuperSkill
            IsPrepareSuperSkill = _inputData.superSkillPrepare;
            if (IsPrepareSuperSkill)
            {
                _accumulatedInput.SuperSkillPrepare = true;
            }

            // Do
            // Attack
            IsAttack = _inputData.doAttack;
            if (IsAttack)
            {
                _accumulatedInput.Attack = true;
            }
            // Skill
            IsSkill = _inputData.doSkill;
            if (IsSkill)
            {
                _accumulatedInput.Skill = true;
            }
            // SuperSkill
            IsSuperSkill = _inputData.doSuperSkill;
            if (IsSuperSkill)
            {
                _accumulatedInput.SuperSkill = true;
            }

            // Cancel
            if (_inputData.doCancel)
            {
                _accumulatedInput.Cancel = true;
            }

            //
            _accumulatedInput.AimDirection = _inputData.aimVector.ToFPVector2();
            _accumulatedInput.AimLength = _accumulatedInput.AimDirection.Magnitude;
        }

        private void PollInput(CallbackPollInput callback)
        {
            AccumulateInput();
            JLog.Warning(_accumulatedInput.MoveDirection);
            callback.SetInput(_accumulatedInput, DeterministicInputFlags.Repeatable);
            _resetAccumulatedInput = true;
        }

        private FPVector2 GetDirectionToMouse()
        {
            if (QuantumRunner.Default == null || QuantumRunner.Default.Game == null)
                return default;

            Frame frame = QuantumRunner.Default.Game.Frames.Predicted;
            if (frame == null)
                return default;

            EntityRef localPlayer = EntityViewSpawner.LocalPlayerEntityRef;
            if (!frame.Exists(EntityViewSpawner.LocalPlayerEntityRef))
                return default;

            Transform2D trans = frame.Get<Transform2D>(localPlayer);
            FPVector2 playerPos = trans.Position;

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            UnityEngine.Plane plane = new UnityEngine.Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out var enter))
            {
                return (ray.GetPoint(enter).ToFPVector2() - playerPos);
            }
            return default;
        }
    }

}