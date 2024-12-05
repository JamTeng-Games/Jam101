using NewGraph;
using System;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.PlayAnimationNode, nodeName: "播放动画", categories: NodeCategory.Timeline)]
    public class PlayAnimationNode : TimelineNode
    {
    }

}