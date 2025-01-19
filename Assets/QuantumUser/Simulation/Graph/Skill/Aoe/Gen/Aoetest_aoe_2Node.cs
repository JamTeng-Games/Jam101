using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.AoeNode, nodeName: "测试aoe2", categories: NodeCategory.Aoe)]
    public unsafe class Aoetest_aoe_2Node : AoeNode
    {

        public override AoeModel ToAoeModel(Frame f)
        {
            AoeModel model = base.ToAoeModel(f);
            model.type = (int)AoeType.test_aoe_2;
            var aoem = new AOEM_Instance();

            model.instance = aoem;
            return model;
        }
    }

}