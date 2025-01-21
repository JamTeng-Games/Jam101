using System;
using System.Collections.Generic;
using NewGraph;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum.Graph.Skill
{

    [Serializable]
    public enum MoveType
    {
        None = 0,
        Ground = 1,
        Fly = 2
    }

    public enum TweenType
    {
        None,
        Linear,
    }

    [Serializable]
    public class BulletNode : NodeBase
    {
        [GraphDisplay(DisplayType.BothViews)]
        public int radius; // 碰撞半径 / 1000

        [GraphDisplay(DisplayType.BothViews)]
        public int sameTargetDelayFrame = 30; // 碰触同一个目标的延迟, 帧数

        [GraphDisplay(DisplayType.BothViews)]
        public MoveType moveType = MoveType.Ground; // 移动方式 fly or ground

        [GraphDisplay(DisplayType.BothViews)]
        public bool removeOnObstacle = true; // 是否碰到障碍物就移除

        [GraphDisplay(DisplayType.BothViews)]
        public bool hitFoe = true; // 是否可以击中敌人

        [GraphDisplay(DisplayType.BothViews)]
        public bool hitAlly = false; // 是否可以击中友军

        [GraphDisplay(DisplayType.BothViews)]
        public TweenType tweenType = TweenType.Linear; // 子弹移动函数的Id

        [GraphDisplay(DisplayType.BothViews)]
        public bool useFireAngle = false;

        [GraphDisplay(DisplayType.BothViews)]
        public List<BulletTag> tags;

        public virtual BulletModel ToBulletModel(Frame f)
        {
            BulletModel model = new BulletModel()
            {
                radius = radius / FP._1000,
                sameTargetDelayFrame = sameTargetDelayFrame,
                moveType = (int)moveType,
                removeOnObstacle = removeOnObstacle,
                hitFoe = hitFoe,
                hitAlly = hitAlly,
                tweenType = (int)tweenType,
                useFireAngle = useFireAngle,
            };

            // tags
            var tags = f.AllocateList<int>(8);
            for (int i = 0; i < this.tags.Count; i++)
            {
                tags.Add((int)this.tags[i]);
            }
            model.tags = tags;
            return model;
        }
    }

}