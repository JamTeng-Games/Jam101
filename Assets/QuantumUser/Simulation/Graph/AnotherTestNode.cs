using System;
using System.Collections.Generic;
using NewGraph;
using UnityEditor;
using UnityEngine;

namespace Quantum.Graph
{

    // Make sure your class is serializable
    // And add the Node attribute. Here you can define a special color for your node as well as organize it with subcategories.
    // [Serializable, Node("#007F00FF", "Special")]
    public class AnotherTestNode : INode
    {
        // [Port, SerializeReference]
        // public TestNode input;

        [Port, SerializeReference]
        public AnotherTestNode anotherInput;

        // Make sure to implement the INode interface so the graph can serialize this easily.
        // with the Portattribute you can create visual connectable ports in the graph view to connect to other nodes.
        // it is important to also add the SerializeReference attribute, so that unity serializes this field as a reference.
        [Port, SerializeReference]
        public TestNode output = null;

        // With the PortList attribute you can instruct the graph view to visualize a dynamic list of ports. 
        [PortList, SerializeReference]
        public List<TestNode> dataList;

        [Serializable]
        public class SpecialData
        {
            [Port, SerializeReference] // You can assign ports and all other attributes in nested classes!
            public TestNode anotherOutput = null;
            // Per default, all data will show up automatically in the side inspector in the GraphView.
            // if you wish to display something directly on the node use the GraphDisplay attribute.
            [GraphDisplay(DisplayType
                .BothViews)] // DisplayType.BothViews visualizes this field both in the inspector and the node itself. 
            public bool toggle;
        }

        public SpecialData specialData;
    }

    public class TestNode : INode
    {
        [GraphDisplay(DisplayType.BothViews)]
        public int value;

        [Port, SerializeReference]
        public TestNode output;
    }

}