using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "测试1", categories: NodeCategory.Buff)]
    public unsafe class BuffTest1Node : BuffNode
    {

        public override BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = base.ToBuffModel(f);
            model.type = (int)BuffType.Test1;
            var bm = new BM_Instance();

            model.instance = bm;
            return model;
        }
    }

}