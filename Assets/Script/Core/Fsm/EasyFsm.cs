using System.Collections.Generic;
using System.Linq;

namespace Jam.Core
{

    /// <summary>
    /// A simple state machine implementation.
    /// </summary>
    /// <typeparam name="TState"> State Type </typeparam>
    public class EasyFsm<TState>
    {
        #region Internal Class

        public class State
        {
            public readonly TState Type;
            private EasyFsm<TState> _fsm;
            private System.Action<TState> _enterAction;
            private System.Action _exitAction;
            private System.Action<float> _tickAction;
            private System.Action _fixedTickAction;
            private List<TState> _transitions;

            public State(TState type, EasyFsm<TState> fsm)
            {
                Type = type;
                _fsm = fsm;
                _transitions = new List<TState>(2);
            }

            public void OnEnter(TState from)
            {
                _enterAction?.Invoke(from);
            }

            public void OnTick(float deltaTime)
            {
                _tickAction?.Invoke(deltaTime);
            }

            public void OnFixedTick()
            {
                _fixedTickAction?.Invoke();
            }

            public void OnExit()
            {
                _exitAction?.Invoke();
            }

            public void ChangeState(TState state)
            {
                if (!HasTransition(state))
                    return;
                _fsm.ChangeState(state);
            }

            public bool HasTransition(TState state)
            {
                foreach (var itState in _transitions)
                {
                    if (EqualityComparer<TState>.Default.Equals(itState, state))
                    {
                        return true;
                    }
                }
                return false;
            }

            public void AddTransition(TState to)
            {
                _transitions.Add(to);
            }

            public void SetEnterAction(System.Action<TState> action)
            {
                _enterAction = action;
            }

            public void SetExitAction(System.Action action)
            {
                _exitAction = action;
            }

            public void SetTickAction(System.Action<float> action)
            {
                _tickAction = action;
            }

            public void SetFixedTickAction(System.Action action)
            {
                _fixedTickAction = action;
            }

            public void Clear()
            {
                _enterAction = null;
                _exitAction = null;
                _tickAction = null;
                _transitions.Clear();
            }
        }

        // Transition Config
        public class TransitionConfig
        {
            private readonly State _from;
            private readonly EasyFsm<TState> _machine;

            public TransitionConfig(State from, EasyFsm<TState> machine)
            {
                _from = from;
                _machine = machine;
            }

            public TransitionConfig To(TState nextState)
            {
                if (_from.HasTransition(nextState))
                    return this;

                if (_machine.HasState(nextState))
                    _from.AddTransition(nextState);
                return this;
            }

            public TransitionConfig OnEnter(System.Action<TState> action)
            {
                _from.SetEnterAction(action);
                return this;
            }

            public TransitionConfig OnTick(System.Action<float> action)
            {
                _from.SetTickAction(action);
                return this;
            }

            public TransitionConfig OnFixedTick(System.Action action)
            {
                _from.SetFixedTickAction(action);
                return this;
            }

            public TransitionConfig OnExit(System.Action action)
            {
                _from.SetExitAction(action);
                return this;
            }
        }

        #endregion Internal Class

        private List<State> _states;
        private State _currentState;
        private bool _isStarted = false;

        public bool IsStarted => _isStarted;
        public TState CurrentState => _currentState.Type;

        public EasyFsm()
        {
            if (!typeof(TState).IsEnum)
            {
                throw new System.Exception("TState must be enum type");
            }

            var states = System.Enum.GetValues(typeof(TState)).Cast<TState>().ToList();
            _states = new List<State>();
            for (int i = 0; i < states.Count; i++)
            {
                State state = new State(states[i], this);
                _states.Add(state);
            }
        }

        public EasyFsm(List<TState> states)
        {
            _states = new List<State>();
            for (int i = 0; i < states.Count; i++)
            {
                State state = new State(states[i], this);
                _states.Add(state);
            }
        }

        public void Start(TState initialState)
        {
            State state = GetState(initialState);
            if (state == null)
            {
                // ERROR!!
                return;
            }
            _currentState = state;
            _currentState.OnEnter(default);
            _isStarted = true;
        }

        public void Tick(float deltaTime)
        {
            if (_isStarted)
                _currentState.OnTick(deltaTime);
        }

        public void FixedTick()
        {
            if (_isStarted)
                _currentState.OnFixedTick();
        }

        public TransitionConfig Configure(TState state)
        {
            State findState = GetState(state);
            if (findState != null)
            {
                return new TransitionConfig(findState, this);
            }
            // ERROR! ThrowException
            State newState = new State(state, this);
            _states.Add(newState);
            return new TransitionConfig(newState, this);
        }

        public State GetState(TState state)
        {
            for (var i = 0; i < _states.Count; i++)
            {
                if (EqualityComparer<TState>.Default.Equals(_states[i].Type, state))
                {
                    return _states[i];
                }
            }
            return null;
        }

        public bool HasState(TState state)
        {
            return GetState(state) != null;
        }

        private void ChangeState(TState newStateType)
        {
            // Same state
            if (EqualityComparer<TState>.Default.Equals(_currentState.Type, newStateType))
                return;

            var newState = GetState(newStateType);
            if (newState == null)
                return;

            // Change state
            var oldState = _currentState;
            _currentState.OnExit();
            _currentState = newState;
            _currentState.OnEnter(oldState.Type);
        }

        public void ForceChangeState(TState newStateType)
        {
            ChangeState(newStateType);
        }

        public void Destroy()
        {
            if (_currentState != null)
                _currentState.OnExit();
            _isStarted = false;
            for (int i = 0; i < _states.Count; i++)
            {
                _states[i].Clear();
            }
            _states.Clear();
            _currentState = null;
        }
    }

}