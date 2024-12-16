using System;
using System.Collections.Generic;

namespace Jam.Core
{

    public partial class Fsm // TODO: IReference
    {
        private int _id;
        private List<State> _states;
        private State _currentState;
        private bool _isRunning;
        private Type _nextState;
        private Blackboard _data;

        public int Id => _id;
        public State CurrentState => _currentState;
        public bool IsRunning => _isRunning;
        public Blackboard Data => _data;

        public Fsm(int id)
        {
            _id = id;
            _states = new List<State>();
            _data = new Blackboard();
        }

        public Fsm(List<State> states, int id)
        {
            _id = id;
            _states = new List<State>(states);
            _data = new Blackboard();
        }

        public void Dispose()
        {
            Stop();
            _states.Clear(); // TODO: Call state's Drop method
            _currentState = null;
        }

        public void Start<TState>() where TState : State
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

        public Fsm AddState(State state)
        {
            state.SetFsm(this);
            _states.Add(state);
            return this;
        }

        public State GetState<TState>()
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

        public State GetState(Type stateType)
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

        public TransitionConfig Configure<TState>()
        {
            var state = GetState<TState>();
            if (state == null)
            {
                throw new Exception($"Can not find state {typeof(TState).Name}");
            }
            return new TransitionConfig(state);
        }

        // 强制切换状态
        public void ForceChangeState<TState>() where TState : State
        {
            if (!_isRunning)
                return;
            _nextState = typeof(TState);
        }

        // 强制立即切换状态
        public void ForceChangeStateNow<TState>() where TState : State
        {
            if (!_isRunning)
                return;
            ChangeStateImpl(typeof(TState));
        }

        public void Tick(float dt)
        {
            if (!_isRunning)
                return;

            // 在帧开头切换状态
            if (_nextState != null)
            {
                ChangeStateImpl(_nextState);
            }

            _currentState.OnTick(dt);
        }

        public void FixedTick()
        {
            if (!_isRunning)
                return;
            // 如果下一帧要切换状态，那么不执行当前状态的FixedTick
            if (_nextState == null)
                _currentState.OnFixedTick();
        }

        public void LateTick()
        {
            if (!_isRunning)
                return;
            // 如果下一帧要切换状态，那么不执行当前状态的LateTick
            if (_nextState == null)
                _currentState.OnLateTick();
        }

        // Private
        /// 等到下一帧再切换状态
        private void ChangeState<TState>() where TState : State
        {
            if (!_isRunning)
                return;
            if (!_currentState.HasTransition<TState>())
                return;
            _nextState = typeof(TState);
        }

        /// 立即切换状态
        private void ChangeStateNow<TState>() where TState : State
        {
            if (!_isRunning)
                return;
            if (!_currentState.HasTransition<TState>())
                return;
            ChangeStateImpl(typeof(TState));
        }

        private void ChangeStateImpl(Type stateType)
        {
            if (!_isRunning)
                return;

            _nextState = null;
            var oldState = _currentState;
            _currentState?.OnExit();
            _currentState = GetState(stateType);
            if (_currentState == null)
                throw new Exception($"Can not find state {stateType.Name}");
            _currentState.OnEnter(oldState);
        }

        // Transition config class
        public class TransitionConfig
        {
            private State _fromState;

            public TransitionConfig(State fromState)
            {
                _fromState = fromState;
            }

            public TransitionConfig To<TState>()
            {
                _fromState.AddTransition(typeof(TState));
                return this;
            }
        }
    }

}