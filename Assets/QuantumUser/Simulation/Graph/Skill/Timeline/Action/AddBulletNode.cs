using System;
using NewGraph;
using UnityEngine;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.AddBulletNode, nodeName: "添加子弹", categories: NodeCategory.Timeline)]
    public class AddBulletNode : TimelineNode
    {
        [Port("子弹"), SerializeReference]
        public BulletNode bulletNode;
    }

}