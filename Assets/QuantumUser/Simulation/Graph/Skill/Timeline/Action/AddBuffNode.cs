using System;
using NewGraph;
using UnityEngine;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.AddBuffNode, nodeName: "添加Buff给释放者", categories: NodeCategory.Timeline)]
    public class AddBuffToCasterNode : TimelineNode
    {
        [Port, SerializeReference]
        public BuffNode buffNode;
    }

}