using System;
using System.Collections.Generic;

namespace J.Core
{
    public class Fsm<TOwner>
    {
        private TOwner _owner;
        private List<FsmState<TOwner>> _states;
        private FsmState<TOwner> _currentState;
        private bool _isRunning;
        private Type _nextState;

        public TOwner Owner => _owner;
        public FsmState<TOwner> CurrentState => _currentState;
        public bool IsRunning => _isRunning;

        public Fsm(TOwner owner)
        {
            _owner = owner;
            _states = new List<FsmState<TOwner>>();
        }

        public Fsm(TOwner owner, List<FsmState<TOwner>> states)
        {
            _owner = owner;
            _states = new List<FsmState<TOwner>>(states);
        }

        public void Drop()
        {
            Stop();
            _states.Clear(); // TODO: Call state's Drop method
            _currentState = null;
            _owner = default;
        }

        public void Start<TState>() where TState : FsmState<TOwner>
        {
            if (_isRunning)
                return;

            _isRunning = true;
            _currentState = GetState<TState>();
            _currentState?.OnEnter(null);
        }

        public void Stop()
        {
            if (!_isRunning)
                return;

            _isRunning = false;
            _currentState?.OnExit();
            _currentState = null;
        }

        public Fsm<TOwner> AddState(FsmState<TOwner> state)
        {
            state.SetFsm(this);
            _states.Add(state);
            return this;
        }

        public FsmState<TOwner> GetState<TState>()
        {
            foreach (var state in _states)
            {
                if (state is TState)
                {
                    return state;
                }
            }
            return null;
        }

        public FsmState<TOwner> GetState(Type stateType)
        {
            foreach (var state in _states)
            {
                if (state.GetType() == stateType)
                {
                    return state;
                }
            }
            return null;
        }

        // 等到一帧结束后再切换状态
        public void ChangeState<TState>() where TState : FsmState<TOwner>
        {
            if (!_isRunning)
                return;

            _nextState = typeof(TState);
        }

        private void ChangeStateImpl(Type stateType)
        {
            if (!_isRunning)
                return;

            var oldState = _currentState;
            _currentState?.OnExit();
            _currentState = GetState(stateType);
            _currentState.OnEnter(oldState);
            _nextState = null;
        }

        public void Tick(float dt)
        {
            if (!_isRunning)
                return;
            _currentState.OnTick(dt);
        }

        public void FixedTick()
        {
            if (!_isRunning)
                return;
            _currentState.OnFixedTick();
        }

        public void LateTick()
        {
            if (!_isRunning)
                return;
            _currentState.OnLateTick();

            if (_nextState != null)
            {
                ChangeStateImpl(_nextState);
            }
        }
    }
}