using System;
using NewGraph;
using UnityEngine;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.CreateAoeNode, nodeName: "添加AOE", categories: NodeCategory.Timeline)]
    public class CreateAoeNode : TimelineNode
    {
        [Port("AOE"), SerializeReference]
        public AoeNode aoeNode;
    }

}