using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace J.Runtime.Input
{

    public abstract class InputMode
    {
        protected IInputActionCollection _input;

        private readonly SortedList<string, InputActionState> _inputActionStates = new SortedList<string, InputActionState>();

        public string Name => GetType().Name;
        public SortedList<string, InputActionState> InputActionStates => _inputActionStates;

        public static TMode Create<TMode>(IInputActionCollection input) where TMode : InputMode, new()
        {
            TMode state = new TMode();
            state._input = input;
            state.Configure();
            return state;
        }

        protected abstract void Configure();

        public void Dispose()
        {
            foreach (var (id, action) in _inputActionStates)
            {
                action.Dispose();
            }
        }

        public void Clear()
        {
            _inputActionStates.Clear();
        }

        protected void EnableInputAction(InputAction action)
        {
            if (_inputActionStates.TryGetValue(action.id.ToString(), out InputActionState state))
            {
                state.isEnabled = true;
                return;
            }
            _inputActionStates.Add(action.id.ToString(), InputActionState.Create(action.id.ToString(), true));
        }

        protected void DisableInputAction(InputAction action)
        {
            if (_inputActionStates.TryGetValue(action.id.ToString(), out InputActionState state))
            {
                state.isEnabled = false;
                return;
            }
            _inputActionStates.Add(action.id.ToString(), InputActionState.Create(action.id.ToString(), false));
        }

        protected void EnableInputActionMap(InputActionMap actionMap)
        {
            foreach (var action in actionMap)
            {
                EnableInputAction(action);
            }
        }

        protected void DisableInputActionMap(InputActionMap actionMap)
        {
            foreach (var action in actionMap)
            {
                DisableInputAction(action);
            }
        }

        protected void EnableAll()
        {
            foreach (var action in _input)
            {
                EnableInputAction(action);
            }
        }

        protected void DisableAll()
        {
            foreach (var action in _input)
            {
                DisableInputAction(action);
            }
        }
    }

}