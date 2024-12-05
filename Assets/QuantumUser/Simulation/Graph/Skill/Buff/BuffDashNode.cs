using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "冲刺", categories: NodeCategory.Buff)]
    public class BuffDashNode : BuffNode
    {
        [GraphDisplay(DisplayType.BothViews)]
        public FP Distance;
    }

}