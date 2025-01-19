using System;
using System.Collections.Generic;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable]
    public class AoeNode : NodeBase
    {
        [GraphDisplay(DisplayType.BothViews)]
        public int radius;
        [GraphDisplay(DisplayType.BothViews)]
        public int tickTime;
        [GraphDisplay(DisplayType.BothViews)]
        public bool removeOnObstacle = false;
        [GraphDisplay(DisplayType.BothViews)]
        public TweenType tweenType = TweenType.Linear;
        [GraphDisplay(DisplayType.BothViews)]
        public List<AoeTag> tags = new List<AoeTag>();

        public virtual AoeModel ToAoeModel(Frame f)
        {
            AoeModel model = new AoeModel()
            {
                radius = radius / FP._1000,
                removeOnObstacle = removeOnObstacle,
                tweenType = (int)tweenType,
                tickTime = tickTime,
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