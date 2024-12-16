using Cinemachine;
using Quantum;

namespace Jam.Arena
{

    public class ArenaContext : QuantumMonoBehaviour, IQuantumViewContext
    {
        public CinemachineVirtualCamera followCamera;

        public AnimationBank animationBank;
    }

}