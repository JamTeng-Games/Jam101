using System;
using NewGraph;

namespace Quantum.Graph.Skill
{

    [Serializable]
    public class TimelineNode : NodeBase
    {
        [GraphDisplay(DisplayType.BothViews)]
        public int frame;
    }

}