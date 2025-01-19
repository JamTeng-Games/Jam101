using System;
using NewGraph;
using UnityEngine;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.CreateAoeNode, nodeName: "创建AOE", categories: NodeCategory.Timeline)]
    public class CreateAoeNode : TimelineNode
    {
        [Port("AOE"), SerializeReference]
        public AoeNode aoeNode;

        [GraphDisplay(DisplayType.BothViews)]
        public int speed;
        [GraphDisplay(DisplayType.BothViews)]
        public int duration;

        public CreateAoeInfo ConvertToCreateAoeInfo(Frame f)
        {
            CreateAoeInfo info = new CreateAoeInfo();
            info.speed = speed;
            info.duration = duration;
            info.model = aoeNode.ToAoeModel(f);
            return info;
        }
    }

}