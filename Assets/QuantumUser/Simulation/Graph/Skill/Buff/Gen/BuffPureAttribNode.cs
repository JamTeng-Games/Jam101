using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "纯属性buff", categories: NodeCategory.Buff)]
    public unsafe class BuffPureAttribNode : BuffNode
    {

        public override BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = base.ToBuffModel(f);
            model.type = (int)BuffType.PureAttrib;
            var bm = new BM_Instance();

            model.instance = bm;
            return model;
        }
    }

}