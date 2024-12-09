using System;
using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class BuffSystem : SystemMainThreadFilter<BuffSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public BuffComp* BuffComp;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var buffs = f.ResolveList(filter.BuffComp->Buffs);
            Span<int> toRemove = stackalloc int[buffs.Count];
            int toRemoveCount = 0;
            for (int i = 0; i < buffs.Count; i++)
            {
                BuffObj buff = buffs[i];
                if (!buff.isPermanent)
                    buff.remainFrame--;
                buff.elapsedFrame++;

                if (buff.model.interval > 0 && buff.elapsedFrame % buff.model.interval == 0)
                {
                    Helper_Buff.OnTick(f, filter.Entity, ref buff);
                    buff.tickTimes++;
                }

                if (buff.remainFrame <= 0 || buff.stack <= 0)
                {
                    Helper_Buff.OnRemove(f, filter.Entity, ref buff);
                    toRemove[toRemoveCount++] = i;
                }
                buffs[i] = buff;
            }

            // Remove Buff
            if (toRemoveCount > 0)
            {
                for (int i = 0; i < toRemoveCount; i++)
                {
                    buffs.RemoveAt(toRemove[i]);
                }
            }

            //
            Helper_Attrib.Recalculate(f, filter.Entity);
        }
    }

}