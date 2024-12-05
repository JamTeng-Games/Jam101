using System;
using System.Collections.Generic;
using NewGraph;
using UnityEngine;

namespace Quantum.Graph.Skill
{

    [Serializable, Node("#007F00FF", nodeName: "时间轴", inputPortCapacity = Capacity.Single)]
    public class Timeline : NodeBase
    {
        [GraphDisplay(DisplayType.BothViews)]
        public TimelineId id;

        [GraphDisplay(DisplayType.BothViews)]
        public int totalFrame;

        [PortList, SerializeReference]
        public List<TimelineNode> nodes;
    }

}