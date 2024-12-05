using System;
using NewGraph;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "自动装填普通攻击子弹", categories: NodeCategory.Buff)]
    public class BuffAutoReloadAttackNode : BuffNode
    {
    }

}