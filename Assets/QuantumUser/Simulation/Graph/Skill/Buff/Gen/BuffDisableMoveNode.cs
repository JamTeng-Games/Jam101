using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "禁锢", categories: NodeCategory.Buff)]
    public unsafe class BuffDisableMoveNode : BuffNode
    {

        public override BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = base.ToBuffModel(f);
            model.type = (int)BuffType.DisableMove;
            var bm = new BM_Instance();

            model.instance = bm;
            return model;
        }
    }

}