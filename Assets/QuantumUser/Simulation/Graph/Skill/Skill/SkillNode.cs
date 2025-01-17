using System;
using System.Collections.Generic;
using NewGraph;
using Quantum;
using UnityEngine;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.SkillNode, nodeName = "技能", createInputPort = false)]
    public class SkillNode : NodeBase
    {
        [GraphDisplay(DisplayType.BothViews)]
        public int id;

        [GraphDisplay(DisplayType.BothViews)]
        public int cd;

        [GraphDisplay(DisplayType.BothViews)]
        public SkillType skillType;

        /// <summary>
        /// 是否可以学多次
        /// </summary>
        [GraphDisplay(DisplayType.BothViews)]
        public bool canLearnMultiTimes;

        [GraphDisplay(DisplayType.BothViews)]
        public bool canInterrupt;

        [Port(name: "时间轴"), SerializeReference]
        public Timeline timeline;

        [PortList, SerializeReference]
        public List<AddBuffToCasterNode> buffs;

        // 消耗属性
        [GraphDisplay(DisplayType.Inspector)]
        public List<AttributeWrap> costAttributes;

        // 状态条件 (在什么状态下可以释放)
        [GraphDisplay(DisplayType.Inspector)]
        public List<StateType> stateConditions;

        // 技能指示器类型
        [GraphDisplay(DisplayType.BothViews)]
        public IndicatorType indicatorType;
    }

    [Serializable]
    public struct AttributeWrap
    {
        public AttributeType type;
        public int value;
    }

}