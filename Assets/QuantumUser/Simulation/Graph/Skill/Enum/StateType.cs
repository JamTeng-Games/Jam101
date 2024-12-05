using System;

namespace Quantum.Graph.Skill
{

    [Serializable]
    public enum StateType
    {
        None,
        Idle,
        Move,
        Attack,
        Skill,
        Stun,
        Die,
    }

}