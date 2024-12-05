using System;
using NewGraph;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "Buff引用", categories: NodeCategory.Buff)]
    public class BuffRefNode : BuffNode
    {
        [GraphDisplay(DisplayType.BothViews)]
        public int buffId;
    }

}