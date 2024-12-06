using System;
using System.Collections.Generic;
using NewGraph;
using Photon.Deterministic;

namespace Quantum.Graph.Skill
{

    [Serializable]
    public struct BaseAttribute
    {
        public AttributeType type;
        public int value;
    }

    [Serializable]
    public struct PercentAttribute
    {
        public AttributeType type;
        public FP value;
    }

    // BuffModel
    [Serializable]
    public unsafe class BuffNode : NodeBase
    {
        [GraphDisplay(DisplayType.Inspector, editability: Editability.None)]
        public BuffId id;

        // 越小的越先执行，优先级越低
        [GraphDisplay(DisplayType.BothViews)]
        public int priority;
        [GraphDisplay(DisplayType.BothViews)]
        public int maxStack = 1;
        [GraphDisplay(DisplayType.BothViews)]
        public int interval = 8;

        // 状态修改
        [GraphDisplay(DisplayType.BothViews)]
        public bool canUseSkill = true;
        [GraphDisplay(DisplayType.BothViews)]
        public bool canMove = true;

        // 属性加成
        [GraphDisplay(DisplayType.Inspector)]
        public List<BaseAttribute> baseAttribs;
        [GraphDisplay(DisplayType.Inspector)]
        public List<PercentAttribute> percentAttribs;

        // Tag
        [GraphDisplay(DisplayType.Inspector)]
        public List<TagType> tags;

        public virtual BuffModel ToBuffModel(Frame f)
        {
            BuffModel model = new BuffModel()
            {
                priority = priority,
                maxStack = maxStack,
                interval = interval,
                canUseSkill = canUseSkill,
                canMove = canMove,
            };

            // tags
            var tags = f.AllocateList<int>(8);
            for (int i = 0; i < this.tags.Count; i++)
            {
                tags.Add((int)this.tags[i]);
            }
            model.tags = tags;

            // base attribs
            var baseAttr = f.AllocateDictionary<int, int>(8);
            for (int i = 0; i < this.baseAttribs.Count; i++)
            {
                baseAttr.Add((int)this.baseAttribs[i].type, this.baseAttribs[i].value);
            }
            model.baseAttribs = baseAttr;

            // percent attribs
            var percentAttr = f.AllocateDictionary<int, FP>(8);
            for (int i = 0; i < this.percentAttribs.Count; i++)
            {
                percentAttr.Add((int)this.percentAttribs[i].type, this.percentAttribs[i].value);
            }
            model.percentAttribs = percentAttr;

            return model;
        }
    }

}