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

            ////////////////////// Skill //////////////////////

            // 解析技能 skm -> SkillModel
            SkillModel skm = new SkillModel()
            {
                id = g_skillNode.id,
                cd = g_skillNode.cd,
                type = (int)g_skillNode.skillType,
                canInterrupt = g_skillNode.canInterrupt,
                canLearnMulti = g_skillNode.canLearnMultiTimes,
                indicatorType = (int)g_skillNode.indicatorType,
            };
            if (g_skillNode.timeline != null)
            {
                skm.timelineModelId = g_skillNode.id;
            }
            // 技能消耗的资源
            var skillCosts = f.AllocateList<AttributeCost>(16);
            foreach (AttributeWrap attrib in g_skillNode.costAttributes)
            {
                skillCosts.Add(new AttributeCost() { attrType = (int)attrib.type, cost = attrib.value });
            }
            skm.attrCosts = skillCosts;
            // 技能的addbuff
            var skillAddBuffs = f.AllocateList<AddBuffInfo>();
            foreach (AddBuffToCasterNode addBuff in g_skillNode.buffs)
            {
                skillAddBuffs.Add(addBuff.ConvertToAddBuffInfo(f));
            }
            skm.addBuffs = skillAddBuffs;

            if (!skillDict.TryAdd(skm.id, skm))
            {
                Log.Error($"Skill {skm.id} already exists.");
                return false;
            }

            ////////////////////// Timeline //////////////////////

            // 解析Timeline
            var g_timeline = g_skillNode.timeline;
            if (g_timeline != null)
            {
                // tlm -> TimelineModel
                TimelineModel tlm =
                    new TimelineModel() { id = g_skillNode.id, totalFrame = g_timeline.totalFrame, };
                var tlmNodes = f.AllocateList<TimelineNode>(16);
                // sort
                g_timeline.nodes.Sort((a, b) =>
                {
                    if (a == null && b == null)
                        return 0;
                    if (a == null)
                        return 1;
                    if (b == null)
                        return -1;
                    return a.frame.CompareTo(b.frame);
                });
                for (int i = 0; i < g_timeline.nodes.Count; i++)
                {
                    var g_node = g_timeline.nodes[i];
                    if (g_node == null)
                        continue;
                    TimelineNode tlmNode = new TimelineNode();
                    tlmNode.frame = g_node.frame;
                    tlmNode.node = new TLNode();
                    if (g_node is LogNode g_logNode)
                    {
                        tlmNode.nodeType = ETLNodeType.Log;
                        tlmNode.node.Log->content = g_logNode.msg;
                    }
                    else if (g_node is AddBuffToCasterNode g_addBuffToCasterNode)
                    {
                        tlmNode.nodeType = ETLNodeType.AddBuffToCaster;
                        tlmNode.node.AddBuffToCaster->addBuffInfo = g_addBuffToCasterNode.ConvertToAddBuffInfo(f);
                    }
                    else if (g_node is PlayAnimationNode g_playAnimNode)
                    {
                        tlmNode.nodeType = ETLNodeType.PlayAnim;
                        tlmNode.node.PlayAnim->animKey = (int)g_playAnimNode.animationKey;
                        tlmNode.node.PlayAnim->force = g_playAnimNode.force;
                    }
                    else if (g_node is FireBulletNode g_fireBulletNode)
                    {
                        tlmNode.nodeType = ETLNodeType.FireBullet;
                        tlmNode.node.FireBullet->fireBulletInfo = g_fireBulletNode.ConvertToFireBulletInfo(f);
                    }
                    tlmNodes.Add(tlmNode);
                }
                tlm.nodes = tlmNodes;
                if (!timelineDict.TryAdd(tlm.id, tlm))
                {
                    Log.Error($"Timeline {tlm.id} already exists.");
                    return false;
                }
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