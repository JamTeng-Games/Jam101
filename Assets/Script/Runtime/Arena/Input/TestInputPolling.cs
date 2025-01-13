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

        private void OnEnable()
        {
            _inputMap = new PlayerInputMap();
            _inputMap.Enable();

            _inputMap.Basic.MoveDir.ReadValue<Vector2>();

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

        private void ProcessStandaloneInput()
        {
            // Move dir
            _accumulatedInput.MoveDirection = _inputMap.Basic.MoveDir.ReadValue<Vector2>().normalized.ToFPVector2();

            // Attack
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _accumulatedInput.Attack = true;
                _accumulatedInput.AttackScreenPos = Mouse.current.position.ReadValue().ToFPVector2();
            }
            // Skill
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _accumulatedInput.Skill = true;
                _accumulatedInput.SkillScreenPos = Mouse.current.position.ReadValue().ToFPVector2();
            }
        }

        private void PollInput(CallbackPollInput callback)
        {
            AccumulateInput();

            callback.SetInput(_accumulatedInput, DeterministicInputFlags.Repeatable);

            _resetAccumulatedInput = true;
        }
    }

}