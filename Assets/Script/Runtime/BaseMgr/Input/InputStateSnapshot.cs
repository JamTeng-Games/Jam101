using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Jam.Runtime.Input_
{

    // 输入状态快照
    public class InputStateSnapshot
    {
        private InputMode _attachedMode;
        private SortedList<string, InputActionState> _inputActionStates = new SortedList<string, InputActionState>();

        public InputMode AttachedMode => _attachedMode;
        public SortedList<string, InputActionState> InputActionStates => _inputActionStates;

        public static InputStateSnapshot Create(IInputActionCollection input, InputMode mode)
        {
            InputStateSnapshot snapshot = TakeSnapshot(input);
            snapshot.ApplyMode(mode);
            return snapshot;
        }

        public static InputStateSnapshot CreateFromSnapshot(InputStateSnapshot source, InputMode mode)
        {
            InputStateSnapshot snapshot = Clone(source);
            snapshot.ApplyMode(mode);
            return snapshot;
        }

        public static InputStateSnapshot CreateAllDisableSnapshot(IInputActionCollection input)
        {
            InputStateSnapshot snapshot = new InputStateSnapshot();
            foreach (InputAction action in input)
            {
                snapshot._inputActionStates.Add(action.id.ToString(), InputActionState.Create(action.id.ToString(), false));
            }
            return snapshot;
        }

        private static InputStateSnapshot TakeSnapshot(IInputActionCollection input)
        {
            InputStateSnapshot snapshot = new InputStateSnapshot();
            foreach (InputAction action in input)
            {
                snapshot._inputActionStates.Add(action.id.ToString(), InputActionState.Create(action.id.ToString(), action.enabled));
            }
            return snapshot;
        }

        private static InputStateSnapshot Clone(InputStateSnapshot source)
        {
            InputStateSnapshot snapshot = new InputStateSnapshot();
            foreach (var (id, actionState) in source.InputActionStates)
            {
                snapshot._inputActionStates.Add(id, InputActionState.Create(id, actionState.isEnabled));
            }
            return snapshot;
        }

        public void ApplyMode(InputMode mode)
        {
            _attachedMode = mode;
            foreach (var (id, actionState) in mode.InputActionStates)
            {
                _inputActionStates.TryGetValue(id, out InputActionState snapshotState);
                snapshotState.isEnabled = actionState.isEnabled;
            }
        }

        public void CloneSnapshot(InputStateSnapshot source)
        {
            foreach (var (id, actionState) in source.InputActionStates)
            {
                _inputActionStates[id].isEnabled = actionState.isEnabled;
            }
            ApplyMode(_attachedMode);
        }

        public void Dispose()
        {
            _attachedMode.Dispose();
        }

        public void Clear()
        {
            _inputActionStates.Clear();
        }
    }

}