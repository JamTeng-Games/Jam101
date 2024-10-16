using System.Collections.Generic;
using System.Linq;

namespace J.Core
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
            public readonly TState owner;
            private System.Action<TState> _enterAction;
            private System.Action _exitAction;
            private System.Action<float> _tickAction;
            private System.Action _fixedTickAction;

            public State(TState owner)
            {
                this.owner = owner;
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
        public TState CurrentState => _currentState.owner;

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
                State state = new State(states[i]);
                _states.Add(state);
            }
        }

        public EasyFsm(List<TState> states)
        {
            _states = new List<State>();
            for (int i = 0; i < states.Count; i++)
            {
                State state = new State(states[i]);
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
            State newState = new State(state);
            _states.Add(newState);
            return new TransitionConfig(newState, this);
        }

        public State GetState(TState state)
        {
            for (var i = 0; i < _states.Count; i++)
            {
                if (EqualityComparer<TState>.Default.Equals(_states[i].owner, state))
                {
                    return _states[i];
                }
            }
            return null;
        }

        public bool HasState(TState state)
        {
            for (var i = 0; i < _states.Count; i++)
            {
                if (EqualityComparer<TState>.Default.Equals(_states[i].owner, state))
                {
                    return true;
                }
            }
            return false;
        }

        public void ChangeState(TState state)
        {
            if (EqualityComparer<TState>.Default.Equals(_currentState.owner, state))
                return;

            for (var i = 0; i < _states.Count; i++)
            {
                if (EqualityComparer<TState>.Default.Equals(_states[i].owner, state))
                {
                    var oldState = _currentState;
                    _currentState.OnExit();
                    _currentState = _states[i];
                    _currentState.OnEnter(oldState.owner);
                }
            }
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

    /// <summary>
    /// A simple state machine implementation.
    /// </summary>
    /// <typeparam name="TState"> State Type </typeparam>
    /// <typeparam name="TTrigger"> Trigger Type </typeparam>
    public class EasyFsm<TState, TTrigger>
    {
        #region Internal Class

        private class Transition
        {
            public State to;
            public TTrigger trigger;
        }

        public class State
        {
            public readonly TState owner;
            private System.Action<TState> _enterAction;
            private System.Action _exitAction;
            private System.Action<float> _tickAction;
            private System.Action _fixedTickAction;
            private List<Transition> _transitions;

            public State(TState owner)
            {
                this.owner = owner;
                _transitions = new List<Transition>();
            }

            public State Fire(TTrigger trigger)
            {
                foreach (var transition in _transitions)
                {
                    if (EqualityComparer<TTrigger>.Default.Equals(transition.trigger, trigger))
                    {
                        return transition.to;
                    }
                }
                return null;
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

            public bool HasTrigger(TTrigger trigger)
            {
                foreach (var transition in _transitions)
                {
                    if (EqualityComparer<TTrigger>.Default.Equals(transition.trigger, trigger))
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool CanTrigger(TTrigger trigger)
            {
                foreach (var transition in _transitions)
                {
                    if (EqualityComparer<TTrigger>.Default.Equals(transition.trigger, trigger))
                    {
                        return true;
                    }
                }
                return false;
            }

            public void AddTransition(State to, TTrigger trigger)
            {
                _transitions.Add(new Transition() { to = to, trigger = trigger, });
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
            private readonly EasyFsm<TState, TTrigger> _machine;

            public TransitionConfig(State from, EasyFsm<TState, TTrigger> machine)
            {
                _from = from;
                _machine = machine;
            }

            public TransitionConfig Permit(TState nextState, TTrigger trigger)
            {
                if (_from.HasTrigger(trigger))
                    return this;

                var toState = _machine.GetState(nextState);
                if (toState != null)
                    _from.AddTransition(toState, trigger);
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
        public TState CurrentState => _currentState.owner;

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
                State state = new State(states[i]);
                _states.Add(state);
            }
        }

        public EasyFsm(List<TState> states)
        {
            _states = new List<State>();
            for (int i = 0; i < states.Count; i++)
            {
                State state = new State(states[i]);
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
            State newState = new State(state);
            _states.Add(newState);
            return new TransitionConfig(newState, this);
        }

        public State GetState(TState state)
        {
            for (var i = 0; i < _states.Count; i++)
            {
                if (EqualityComparer<TState>.Default.Equals(_states[i].owner, state))
                {
                    return _states[i];
                }
            }
            return null;
        }

        public bool HasState(TState state)
        {
            for (var i = 0; i < _states.Count; i++)
            {
                if (EqualityComparer<TState>.Default.Equals(_states[i].owner, state))
                {
                    return true;
                }
            }
            return false;
        }

        public void ChangeState(TState state)
        {
            if (EqualityComparer<TState>.Default.Equals(_currentState.owner, state))
                return;

            for (var i = 0; i < _states.Count; i++)
            {
                if (EqualityComparer<TState>.Default.Equals(_states[i].owner, state))
                {
                    var oldState = _currentState;
                    _currentState.OnExit();
                    _currentState = _states[i];
                    _currentState.OnEnter(oldState.owner);
                }
            }
        }

        // Send Trigger
        public void Fire(TTrigger trigger)
        {
            var nextState = _currentState.Fire(trigger);
            if (nextState != null)
            {
                var oldState = _currentState;
                _currentState.OnExit();
                _currentState = nextState;
                _currentState.OnEnter(oldState.owner);
            }
        }

        public bool CanTrigger(TTrigger trigger)
        {
            return _currentState.CanTrigger(trigger);
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