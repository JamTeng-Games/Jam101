using System;
using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class AddBuffSystem : SystemMainThreadFilter<AddBuffSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public BuffComp* BuffComp;
            public StatsComp* StatsComp;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var buffs = f.ResolveList(filter.BuffComp->Buffs);
            var addBuffs = f.ResolveList(filter.BuffComp->AddBuffs);
            for (int i = 0; i < addBuffs.Count; i++)
            {
                Log.Debug($"addBuffs.Count {addBuffs.Count}");
                var addBuff = addBuffs[i];
                BuffObj newBuff = default;
                bool isRemove = false;
                int modStack = Math.Min(addBuff.addStack, addBuff.buffModel.maxStack);
                // 已经存在
                if (Helper_Buff.TryGetBuff(buffs, addBuff.buffModel.type, addBuff.caster, out int oldBuffIndex))
                {
                    var oldBuff = buffs[oldBuffIndex];
                    // model (args)
                    oldBuff.model = addBuff.buffModel; // Model
                    // remain frame
                    if (addBuff.isDurationSetTo)
                    {
                        oldBuff.remainFrame = addBuff.duration;
                    }
                    else
                    {
                        oldBuff.remainFrame += addBuff.duration;
                    }
                    // permanent
                    oldBuff.isPermanent = addBuff.isPermanent;
                    // stack
                    int newStack = Math.Max(Math.Min(oldBuff.stack + addBuff.addStack, addBuff.buffModel.maxStack), 0);
                    modStack = newStack - oldBuff.stack;
                    oldBuff.stack = newStack;
                    isRemove = oldBuff.stack == 0;
                    // 
                    newBuff = oldBuff;
                    buffs[oldBuffIndex] = newBuff;
                }
                // 全新buff
                else
                {
                    if (addBuff.addStack <= 0)
                        continue;

                    newBuff = new BuffObj()
                    {
                        isPermanent = addBuff.isPermanent,
                        remainFrame = addBuff.duration,
                        stack = addBuff.addStack,
                        elapsedFrame = 0,
                        tickTimes = 0,
                        caster = addBuff.caster,
                        owner = filter.Entity,
                        model = addBuff.buffModel
                    };
                    buffs.Add(newBuff);
                }

                if (!isRemove)
                {
                    Helper_Buff.OnAdd(f, filter.Entity, ref newBuff, modStack);
                }
            }
            if (addBuffs.Count > 0)
            {
                // 根据优先级排序
                buffs.Sort(Helper_Buff.BuffCompare.Instance);
                // 重新计算属性
                Helper_Attrib.Recalculate(f, filter.Entity);
            }
            addBuffs.Clear();

            if (!filter.StatsComp->IsInit)
                Helper_Stats.InitStats(f, filter.Entity);
        }
    }

}