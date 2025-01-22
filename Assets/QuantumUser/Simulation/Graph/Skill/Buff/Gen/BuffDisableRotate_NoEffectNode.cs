using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "禁止旋转(无表现)", categories: NodeCategory.Buff)]
    public unsafe class BuffDisableRotate_NoEffectNode : BuffNode
    {

        public override BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = base.ToBuffModel(f);
            model.type = (int)BuffType.DisableRotate_NoEffect;
            var bm = new BM_Instance();

            model.instance = bm;
            return model;
        }
    }

}