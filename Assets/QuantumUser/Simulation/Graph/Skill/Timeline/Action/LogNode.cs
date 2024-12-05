using System;
using NewGraph;
using UnityEngine;

namespace Quantum.Graph.Skill
{

    [Serializable, Node(NodeColor.LogNode, nodeName: "调试日志", categories: NodeCategory.Timeline)]
    public class LogNode : TimelineNode
    {
        [GraphDisplay(DisplayType.BothViews)]
        public string msg;
    }

}