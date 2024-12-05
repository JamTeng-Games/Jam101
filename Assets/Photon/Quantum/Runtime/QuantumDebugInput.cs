namespace Quantum {

  using System;
  using Photon.Deterministic;
  using UnityEngine;

  /// <summary>
  /// A Unity script that creates empty input for any Quantum game.
  /// </summary>
  public class QuantumDebugInput : MonoBehaviour {
    private Quantum.Input _accumulatedInput;
    private bool _resetAccumulatedInput;
    private int _lastAccumulateFrame;

    private void OnEnable() {
      QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    private void Update() {
      AccumulateInput();
    }

    private void AccumulateInput() {
      if (_lastAccumulateFrame == Time.frameCount)
        return;

      _lastAccumulateFrame = Time.frameCount;

      if (_resetAccumulatedInput) {
        _resetAccumulatedInput = false;
        _accumulatedInput = default;
      }

      ProcessStandaloneInput();
    }

    private void ProcessStandaloneInput() {
    }

    private void PollInput(CallbackPollInput callback) {
      AccumulateInput();

      callback.SetInput(_accumulatedInput, DeterministicInputFlags.Repeatable);

      _resetAccumulatedInput = true;
    }
  }

}