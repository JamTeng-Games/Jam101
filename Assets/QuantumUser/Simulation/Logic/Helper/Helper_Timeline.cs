namespace Quantum.Helper
{

    public static unsafe class Helper_Timeline
    {
        public static void AddTimeline(Frame f, TimelineModel model, EntityRef caster, bool force)
        {
            TimelineObj tlObj = CreateTimelineObj(model, caster);
            AddTimelineObj(f, tlObj, force);
        }

        public static void AddTimelineObj(Frame f, TimelineObj tlObj, bool force)
        {
            if (f.Unsafe.TryGetPointerSingleton<STimelineComp>(out var tlComp))
            {
                if (tlObj.caster == EntityRef.None)
                    return;
                var tlObjs = f.ResolveList(tlComp->Timelines);
                if (CasterHasTimeline(f, tlObj.caster, out int index))
                {
                    if (!force)
                        return;
                    var oldTlObj = tlObjs[index];
                    oldTlObj.elapsedFrame = oldTlObj.model.totalFrame;
                    tlObjs[index] = oldTlObj;
                }
                tlObjs.Add(tlObj);
            }
        }

        public static bool TryGetTimelineModel(Frame f, int timelineId, out TimelineModel model)
        {
            model = default;
            if (f.TryGetSingleton<STimelineModelContainerComp>(out var tlModelComp))
            {
                var tlModels = f.ResolveDictionary(tlModelComp.Models);
                return tlModels.TryGetValue(timelineId, out model);
            }
            return false;
        }

        public static bool CasterHasTimeline(Frame f, EntityRef caster, out int index)
        {
            index = -1;
            if (f.TryGetSingleton<STimelineComp>(out var tlComp))
            {
                var tlObjs = f.ResolveList(tlComp.Timelines);
                for (int i = 0; i < tlObjs.Count; i++)
                {
                    var tlObj = tlObjs[i];
                    if (tlObj.caster == caster)
                    {
                        index = i;
                        return true;
                    }
                }
            }
            return false;
        }

        public static TimelineObj CreateTimelineObj(TimelineModel model, EntityRef caster)
        {
            return new TimelineObj { model = model, caster = caster, };
        }
    }

}