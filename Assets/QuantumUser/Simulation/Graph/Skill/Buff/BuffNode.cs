using System;
using NewGraph;

namespace Quantum.Graph.Skill
{

    [Serializable]
    public class BuffNode : NodeBase
    {
#if UNITY_EDITOR
        [GraphDisplay(DisplayType.BothViews)]
        public string comment;
#endif
    }

}