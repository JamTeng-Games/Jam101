namespace Quantum.Helper
{

    public static unsafe class Helper_Timeline
    {
        public static void AddTimeline(Frame f, TimelineModel model, EntityRef caster)
        {
            TimelineObj tlObj = CreateTimelineObj(model, caster);
            AddTimelineObj(f, tlObj);
        }

        public static void AddTimelineObj(Frame f, TimelineObj tlObj)
        {
            if (f.Unsafe.TryGetPointerSingleton<STimelineComp>(out var tlComp))
            {
                if (tlObj.caster == EntityRef.None && CasterHasTimeline(f, tlObj.caster))
                    return;

                var tlObjs = f.ResolveList(tlComp->Timelines);
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

        public static bool CasterHasTimeline(Frame f, EntityRef caster)
        {
            if (f.TryGetSingleton<STimelineComp>(out var tlComp))
            {
                var tlObjs = f.ResolveList(tlComp.Timelines);
                foreach (var tlObj in tlObjs)
                {
                    if (tlObj.caster == caster)
                        return true;
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