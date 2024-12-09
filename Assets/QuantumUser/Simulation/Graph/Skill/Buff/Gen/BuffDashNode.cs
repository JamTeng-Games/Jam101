using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "冲刺", categories: NodeCategory.Buff)]
    public unsafe class BuffDashNode : BuffNode
    {
        [GraphDisplay(DisplayType.BothViews)] public FP distance;

        public override BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = base.ToBuffModel(f);
            model.type = (int)BuffType.Dash;
            var bm = new BM_Instance();
            bm.Dash->distance = distance;

            model.instance = bm;
            return model;
        }
    }

}