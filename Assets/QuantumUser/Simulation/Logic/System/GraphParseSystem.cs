using Quantum.Collections;
using Quantum.Graph.Skill;
using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    /// 解析技能图
    [Preserve]
    public unsafe class GraphParseSystem : SystemSignalsOnly
    {
        public override void OnInit(Frame f)
        {
            var skillDict = f.AllocateDictionary<int, SkillModel>(32);
            var timelineDict = f.AllocateDictionary<int, TimelineModel>(32);

            foreach (var skillGraphRef in f.RuntimeConfig.SkillGraphs)
            {
                var skillGraph = f.FindAsset<AssetObjectGraphModel>(skillGraphRef);
                if (Helper_GraphParser.TryParseSkill(f, skillGraph, skillDict, timelineDict))
                {
                    // Log
                    Log.Debug("Skill Graph Parsed Successfully");
                    foreach (var (skId, skm) in skillDict)
                    {
                        Log.Debug($"Skill: {(SkillId)skId} - {skm.cd} {skm.isAttack}");
                    }
                    foreach (var (tlId, tlm) in timelineDict)
                    {
                        var nL = f.ResolveList(tlm.nodes);
                        foreach (var n in nL)
                        {
                            Log.Debug($"Timeline: {tlId} - {n.nodeType} {n.frame}");
                        }
                    }
                }
                else
                {
                    Log.Error("Skill Graph Parse Failed");
                }
            }

            var skmComp = f.Unsafe.GetOrAddSingletonPointer<SSkillModelContainerComp>();
            var tlmComp = f.Unsafe.GetOrAddSingletonPointer<STimelineModelContainerComp>();
            skmComp->Models = skillDict;
            tlmComp->Models = timelineDict;
        }
    }

}