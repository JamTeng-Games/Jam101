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

        [GraphDisplay(DisplayType.BothViews)]
        public bool isPermanent;
        [GraphDisplay(DisplayType.BothViews)]
        public int addStack;
        [GraphDisplay(DisplayType.BothViews)]
        public int duration;

        [GraphDisplay(DisplayType.Hide)]
        public EntityRef caster;
        [GraphDisplay(DisplayType.Hide)]
        public EntityRef target;

        public AddBuffInfo ToAddBuffInfo(Frame f)
        {
            AddBuffInfo info = new AddBuffInfo();
            info.isPermanent = isPermanent;
            info.addStack = addStack;
            info.duration = duration;
            info.buffModel = buffNode.ToBuffModel(f);
            return info;
        }
    }

}