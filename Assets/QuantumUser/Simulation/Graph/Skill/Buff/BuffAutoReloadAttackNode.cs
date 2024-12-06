using System;
using NewGraph;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "自动装填普通攻击子弹", categories: NodeCategory.Buff)]
    public unsafe class BuffAutoReloadAttackNode : BuffNode
    {
        [GraphDisplay(DisplayType.BothViews)]
        public int reloadFrame;

        public override BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = base.ToBuffModel(f);
            model.id = (int)BuffId.AutoReload;
            var bm = new BM_Instance();
            bm.autoReloadAttack->reloadFrame = reloadFrame;
            model.instance = bm;
            return model;
        }
    }

}