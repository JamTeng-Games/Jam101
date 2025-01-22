using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "解除禁止旋转一层", categories: NodeCategory.Buff)]
    public unsafe class BuffEnableRotateNode : BuffNode
    {

        public override BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = base.ToBuffModel(f);
            model.type = (int)BuffType.EnableRotate;
            var bm = new BM_Instance();

            model.instance = bm;
            return model;
        }
    }

}