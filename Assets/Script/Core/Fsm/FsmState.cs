using System.Collections.Generic;

namespace Jam.Core
{

    public partial class Fsm // TODO: IReference
    {
        public abstract class State // TODO: IReference
        {
            protected Fsm _fsm;
            private List<System.Type> _transitions = new List<System.Type>(2);

            public abstract void OnEnter(State fromState);
            public abstract void OnExit();
            public abstract void OnTick(float dt);

            public virtual void OnFixedTick()
            {
            }

            public virtual void OnLateTick()
            {
            }

            public void SetFsm(Fsm fsm)
            {
                _fsm = fsm;
            }

            public bool HasTransition<T>() where T : State
            {
                return HasTransition(typeof(T));
            }

            public bool HasTransition(System.Type stateType)
            {
                return _transitions.Contains(stateType);
            }

            protected void ChangeState<T>() where T : State
            {
                _fsm.ChangeState<T>();
            }

            protected void ChangeStateNow<T>() where T : State
            {
                _fsm.ChangeStateNow<T>();
            }

            // Don't call directly!!!
            public void AddTransition(System.Type toState)
            {
                _transitions.Add(toState);
            }
        }
    }

}