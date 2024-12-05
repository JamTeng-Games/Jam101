using System;
using NewGraph;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.CreateDamageNode, nodeName: "造成伤害", categories: NodeCategory.Timeline)]
    public class CreateDamageNode : TimelineNode
    {
    }

}