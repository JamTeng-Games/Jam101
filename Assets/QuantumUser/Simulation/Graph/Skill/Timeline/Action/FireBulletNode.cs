using System;
using NewGraph;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.FireBulletNode, nodeName: "发射子弹", categories: NodeCategory.Timeline)]
    public class FireBulletNode : TimelineNode
    {
        [Port("子弹"), SerializeReference]
        public BulletNode bulletNode;

        [GraphDisplay(DisplayType.BothViews)]
        public int speed; // 10s 飞多远

        [GraphDisplay(DisplayType.BothViews)]
        public int duration;

        [GraphDisplay(DisplayType.BothViews)]
        public int timeCanHit = 10;

        [GraphDisplay(DisplayType.BothViews)]
        public int hitTimes = 10;

        public FireBulletInfo ConvertToFireBulletInfo(Frame f)
        {
            FireBulletInfo info = new FireBulletInfo();
            info.speed = speed;
            info.duration = duration;
            info.timeCanHit = timeCanHit;
            info.hitTimes = hitTimes;
            info.model = bulletNode.ToBulletModel(f);
            return info;
        }
    }

}