using System.Collections.Generic;
using Jam.Core;
using Quantum;

namespace Jam.Arena
{

    public partial class JamEntityView : QuantumEntityView
    {
        private List<IQuantumViewComponent> _viewComps;
        private List<IQuantumViewComponent> _viewCompsToAdd;
        private List<IQuantumViewComponent> _viewCompsToRemove;
        private Signal<ViewSignal> _signal;

        public override void OnInitialize()
        {
            _signal = Signal<ViewSignal>.Create<ViewSignal>();
            _viewComps = new List<IQuantumViewComponent>(16);
            _viewCompsToAdd = new List<IQuantumViewComponent>(16);
            _viewCompsToRemove = new List<IQuantumViewComponent>(16);
        }

        public override void OnActivate(Frame frame)
        {
            for (int i = 0; i < _viewComps.Count; i++)
            {
                _viewComps[i].Activate(frame, Game, this);
            }
        }

        public override void OnDeactivate()
        {
            for (int i = 0; i < _viewComps.Count; i++)
            {
                _viewComps[i].Deactivate();
            }
        }

        public override void OnGameChanged()
        {
            for (int i = 0; i < _viewComps.Count; i++)
            {
                _viewComps[i].GameChanged(Game);
            }
        }

        public override void OnUpdateView()
        {
            for (int i = 0; i < _viewCompsToAdd.Count; i++)
            {
                var vc = _viewCompsToAdd[i];
                vc.Activate(Game.Frames.Verified, Game, this);
                _viewComps.Add(vc);
            }
            _viewCompsToAdd.Clear();

            for (int i = 0; i < _viewComps.Count; i++)
            {
                _viewComps[i].UpdateView();
            }

            _signal.Tick();
        }

        public override void OnLateUpdateView()
        {
            for (int i = 0; i < _viewComps.Count; i++)
            {
                _viewComps[i].LateUpdateView();
            }

            for (int i = 0; i < _viewCompsToRemove.Count; i++)
            {
                _viewComps.Remove(_viewCompsToRemove[i]);
            }
            _viewCompsToRemove.Clear();
        }

        public void AddViewComp(IQuantumViewComponent viewComp)
        {
            if (!viewComp.IsInitialized)
                viewComp.Initialize(ViewContexts);
            _viewCompsToAdd.Add(viewComp);
        }

        public void RemoveViewComp(IQuantumViewComponent viewComp)
        {
            if (_viewComps.Contains(viewComp))
            {
                viewComp.Deactivate();
                _viewCompsToRemove.Add(viewComp);
            }
        }
    }

}