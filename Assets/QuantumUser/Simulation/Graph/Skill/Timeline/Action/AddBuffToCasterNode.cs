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
        public int addStack = 1;
        [GraphDisplay(DisplayType.BothViews)]
        public int duration = 1;
        [GraphDisplay(DisplayType.BothViews)]
        public bool isDurationSetTo;

        [GraphDisplay(DisplayType.Hide)]
        public EntityRef caster;
        [GraphDisplay(DisplayType.Hide)]
        public EntityRef target;

        public AddBuffInfo ConvertToAddBuffInfo(Frame f)
        {
            AddBuffInfo info = new AddBuffInfo();
            info.isPermanent = isPermanent;
            info.addStack = addStack;
            info.duration = duration;
            info.isDurationSetTo = isDurationSetTo;
            info.buffModel = buffNode.ToBuffModel(f);
            return info;
        }
    }

}