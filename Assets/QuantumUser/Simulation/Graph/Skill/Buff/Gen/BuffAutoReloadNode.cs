using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "自动装填普通攻击子弹", categories: NodeCategory.Buff)]
    public unsafe class BuffAutoReloadNode : BuffNode
    {
        [GraphDisplay(DisplayType.BothViews)] public int reloadFrame;

        public override BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = base.ToBuffModel(f);
            model.type = (int)BuffType.AutoReload;
            var bm = new BM_Instance();
            bm.AutoReload->reloadFrame = reloadFrame;

            model.instance = bm;
            return model;
        }
    }

}