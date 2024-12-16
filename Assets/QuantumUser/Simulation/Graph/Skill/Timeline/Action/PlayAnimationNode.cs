using NewGraph;
using System;
using UnityEngine.Serialization;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.PlayAnimationNode, nodeName: "播放动画", categories: NodeCategory.Timeline)]
    public class PlayAnimationNode : TimelineNode
    {
        [GraphDisplay(DisplayType.BothViews)]
        public AnimationKey animationKey;
        [GraphDisplay(DisplayType.BothViews)]
        public bool force;
    }

}