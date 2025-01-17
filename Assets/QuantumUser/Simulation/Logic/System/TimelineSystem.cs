using Quantum.Graph.Skill;
using Quantum.Helper;

namespace Quantum
{

    using Photon.Deterministic;
    using UnityEngine.Scripting;

    /// 运行Timeline
    [Preserve]
    public unsafe class TimelineSystem : SystemMainThread
    {
        public override void OnInit(Frame f)
        {
            f.GetOrAddSingleton<STimelineComp>();
        }

        public override void Update(Frame f)
        {
            if (!f.Unsafe.TryGetPointerSingleton<STimelineComp>(out var tlComp))
                return;

            var tlObjs = f.ResolveList(tlComp->Timelines);
            Log.Debug("Timeline Count: " + tlObjs.Count);
            for (int i = tlObjs.Count - 1; i >= 0; i--)
            {
                var tlObj = tlObjs[i];
                int wasElapsedFrame = tlObj.elapsedFrame;
                tlObj.elapsedFrame++;
                var nodes = f.ResolveList(tlObj.model.nodes);
                for (int j = 0; j < nodes.Count; j++)
                {
                    TimelineNode node = nodes[j];
                    if (wasElapsedFrame == node.frame)
                    {
                        Helper_TLNode.Execute(f, tlObj, node);
                    }
                }

                if (tlObj.elapsedFrame >= tlObj.model.totalFrame)
                {
                    // Remove
                    tlObjs.RemoveAt(i);
                }
                else
                {
                    // 因为是struct 需要赋值回去
                    tlObjs[i] = tlObj;
                }
            }
        }
    }

}