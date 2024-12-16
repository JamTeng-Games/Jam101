using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BuffNode, nodeName: "沉默", categories: NodeCategory.Buff)]
    public unsafe class BuffDisableSkillNode : BuffNode
    {

        public override BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = base.ToBuffModel(f);
            model.type = (int)BuffType.DisableSkill;
            var bm = new BM_Instance();

            model.instance = bm;
            return model;
        }
    }

}