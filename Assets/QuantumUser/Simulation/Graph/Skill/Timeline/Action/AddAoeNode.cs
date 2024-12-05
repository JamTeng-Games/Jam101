using System;
using NewGraph;
using UnityEngine;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.AddAoeNode, nodeName: "添加AOE", categories: NodeCategory.Timeline)]
    public class AddAoeNode : TimelineNode
    {
        [Port("AOE"), SerializeReference]
        public AoeNode aoeNode;
    }

}