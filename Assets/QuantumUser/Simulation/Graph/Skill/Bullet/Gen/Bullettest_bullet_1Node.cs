using System;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.BulletNode, nodeName: "测试子弹1", categories: NodeCategory.Bullet)]
    public unsafe class Bullettest_bullet_1Node : BulletNode
    {
        [GraphDisplay(DisplayType.BothViews)] public FP arg1;

        public override BulletModel ToBulletModel(Frame f)
        {
            BulletModel model = base.ToBulletModel(f);
            model.type = (int)BulletType.test_bullet_1;
            var bm = new BLTM_Instance();
            bm.test_bullet_1->arg1 = arg1;

            model.instance = bm;
            return model;
        }
    }

}