namespace J.Core
{
    public abstract class FsmState<TOwner>
    {
        protected Fsm<TOwner> _fsm;

        public TOwner Owner => _fsm.Owner;

        public abstract void OnEnter(FsmState<TOwner> fromState);
        public abstract void OnExit();
        public abstract void OnTick(float dt);

        public virtual void OnFixedTick()
        {
        }

        public virtual void OnLateTick()
        {
        }

        public void SetFsm(Fsm<TOwner> fsm)
        {
            _fsm = fsm;
        }
    }
}