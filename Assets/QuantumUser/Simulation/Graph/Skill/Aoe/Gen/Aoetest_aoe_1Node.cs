using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.AoeNode, nodeName: "测试1", categories: NodeCategory.Aoe)]
    public unsafe class Aoetest_aoe_1Node : AoeNode
    {
        [GraphDisplay(DisplayType.BothViews)] public int arc;

        public override AoeModel ToAoeModel(Frame f)
        {
            AoeModel model = base.ToAoeModel(f);
            model.type = (int)AoeType.test_aoe_1;
            var aoem = new AOEM_Instance();
            aoem.test_aoe_1->arc = arc;

            model.instance = aoem;
            return model;
        }
    }

}