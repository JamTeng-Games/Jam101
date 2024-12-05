using System.Collections.Generic;
using Quantum.Collections;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public static unsafe class Helper_GraphParser
    {
        public static bool TryParseGraph(Frame f, AssetObjectGraphModel graph, out List<TimelineModel> timeline)
        {
            timeline = null;
            if (graph == null)
                return false;

            foreach (var node in graph.Nodes)
            {
            }
            return false;
        }

        public static bool TryParseSkill(Frame f,
                                         AssetObjectGraphModel graph,
                                         QDictionary<int, SkillModel> skillDict,
                                         QDictionary<int, TimelineModel> timelineDict)
        {
            // 找到技能节点
            // g -> graph
            SkillNode g_skillNode = null;
            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                var g_node = graph.Nodes[i].nodeData;
                if (g_node is SkillNode)
                {
                    g_skillNode = (SkillNode)g_node;
                    break;
                }
            }

            if (g_skillNode == null)
                return false;

            // 解析技能 skm -> SkillModel
            SkillModel skm = new SkillModel()
            {
                id = (int)g_skillNode.id,
                cd = g_skillNode.cd,
                isAttack = g_skillNode.isAttack,
                indicatorType = (int)g_skillNode.indicatorType,
                timelineModelId = (int)g_skillNode.timeline.id
            };

            if (!skillDict.TryAdd(skm.id, skm))
            {
                Log.Error($"Skill {skm.id} already exists.");
                return false;
            }

            // 解析Timeline
            var g_timeline = g_skillNode.timeline;
            // tlm -> TimelineModel
            TimelineModel tlm = new TimelineModel() { id = (int)g_timeline.id, totalFrame = g_timeline.totalFrame, };
            var tlmNodes = f.AllocateList<TimelineNode>(16);
            // sort
            g_timeline.nodes.Sort((a, b) => a.frame.CompareTo(b.frame));
            for (int i = 0; i < g_timeline.nodes.Count; i++)
            {
                var g_node = g_timeline.nodes[i];
                TimelineNode tlmNode = new TimelineNode();
                tlmNode.frame = g_node.frame;
                if (g_node is LogNode g_logNode)
                {
                    tlmNode.nodeType = ETLNodeType.Log;
                    tlmNode.node = new TLNode();
                    tlmNode.node.Log->content = g_logNode.msg;
                }
                else if (g_node is AddBuffToCasterNode g_addBuffToCasterNode)
                {
                    tlmNode.nodeType = ETLNodeType.None;
                }
                tlmNodes.Add(tlmNode);
            }
            tlm.nodes = tlmNodes;
            if (!timelineDict.TryAdd(tlm.id, tlm))
            {
                Log.Error($"Timeline {tlm.id} already exists.");
                return false;
            }
            return true;
        }

        public static bool TryParseTimeline(AssetObjectGraphModel graph, out TimelineModel timelineModel)
        {
            timelineModel = default;
            return false;
        }
    }

}