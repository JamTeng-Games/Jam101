using System;
using NewGraph;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.StopMoveNode, nodeName: "停止移动", categories: NodeCategory.Timeline)]
    public class StopMoveNode : TimelineNode
    {
    }

}