using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "test_item_1", categories: NodeCategory.Buff)]
    public unsafe class Bufftest_item_1Node : BuffNode
    {

        public override BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = base.ToBuffModel(f);
            model.type = (int)BuffType.test_item_1;
            var bm = new BM_Instance();

            model.instance = bm;
            return model;
        }
    }

}