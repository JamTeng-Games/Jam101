using System;
using NewGraph;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.PlayEffectNode, nodeName: "播放特效", categories: NodeCategory.Timeline)]
    public class PlayEffectNode : TimelineNode
    {
    }

}