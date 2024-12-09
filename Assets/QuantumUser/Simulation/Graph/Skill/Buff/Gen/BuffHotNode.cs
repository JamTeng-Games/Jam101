using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "热区", categories: NodeCategory.Buff)]
    public unsafe class BuffHotNode : BuffNode
    {
        [GraphDisplay(DisplayType.BothViews)] public short tick;
        [GraphDisplay(DisplayType.BothViews)] public ushort area;
        [GraphDisplay(DisplayType.BothViews)] public short damage;

        public override BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = base.ToBuffModel(f);
            model.type = (int)BuffType.Hot;
            var bm = new BM_Instance();
            bm.Hot->tick = tick;
            bm.Hot->area = area;
            bm.Hot->damage = damage;

            model.instance = bm;
            return model;
        }
    }

}