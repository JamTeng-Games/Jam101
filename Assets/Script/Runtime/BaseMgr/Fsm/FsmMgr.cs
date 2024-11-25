using System.Collections.Generic;
using Jam.Core;

namespace Jam.Runtime.Fsm_
{

    public class FsmMgr : IMgr, ITickable, IFixedTickable, ILateTickable
    {
        private static int _fsmIdGenerator = 0;
        private List<Fsm> _fsmList;

        public FsmMgr()
        {
            _fsmList = new List<Fsm>(16);
        }

        public void Shutdown(bool isAppQuit)
        {
        }

        public Fsm CreateFsm(out int id)
        {
            id = ++_fsmIdGenerator;
            Fsm fsm = new Fsm(id);
            AddFsm(fsm);
            return fsm;
        }

        public Fsm CreateFsm(List<Fsm.State> states, out int id)
        {
            id = ++_fsmIdGenerator;
            Fsm fsm = new Fsm(states, id);
            AddFsm(fsm);
            return fsm;
        }

        public bool HasFsm(int id)
        {
            for (int i = _fsmList.Count - 1; i >= 0; i--)
            {
                if (_fsmList[i].Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public Fsm GetFsm(int id)
        {
            for (int i = _fsmList.Count - 1; i >= 0; i--)
            {
                if (_fsmList[i].Id == id)
                {
                    return _fsmList[i];
                }
            }
            return null;
        }

        public void DestroyFsm(int id)
        {
            for (int i = _fsmList.Count - 1; i >= 0; i--)
            {
                if (_fsmList[i].Id == id)
                {
                    _fsmList[i].Dispose();
                    _fsmList.RemoveAt(i);
                    return;
                }
            }
        }

        private void AddFsm(Fsm fsm)
        {
            _fsmList.Add(fsm);
        }

        // Tick
        public void Tick(float deltaTime)
        {
            for (int i = _fsmList.Count - 1; i >= 0; i--)
            {
                _fsmList[i].Tick(deltaTime);
            }
        }

        public void FixedTick()
        {
            for (int i = _fsmList.Count - 1; i >= 0; i--)
            {
                _fsmList[i].FixedTick();
            }
        }

        public void LateTick()
        {
            for (int i = _fsmList.Count - 1; i >= 0; i--)
            {
                _fsmList[i].LateTick();
            }
        }
    }

}