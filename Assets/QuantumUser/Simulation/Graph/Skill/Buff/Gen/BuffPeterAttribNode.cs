using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "PeterAttrib", categories: NodeCategory.Buff)]
    public unsafe class BuffPeterAttribNode : BuffNode
    {

        public override BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = base.ToBuffModel(f);
            model.type = (int)BuffType.PeterAttrib;
            var bm = new BM_Instance();

            model.instance = bm;
            return model;
        }
    }

}