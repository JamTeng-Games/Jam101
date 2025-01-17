using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BulletNode, nodeName: "弓箭", categories: NodeCategory.Bullet)]
    public unsafe class BulletArrowNode : BulletNode
    {

        public override BulletModel ToBulletModel(Frame f)
        {
            BulletModel model = base.ToBulletModel(f);
            model.type = (int)BulletType.Arrow;
            var bm = new BLTM_Instance();

            model.instance = bm;
            return model;
        }
    }

}