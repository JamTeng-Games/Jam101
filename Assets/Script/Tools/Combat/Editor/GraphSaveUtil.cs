using System.Collections.Generic;
using System.Linq;
using Jam.Runtime.Combat_;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jam.Editor_.Combat
{

    public class GraphSaveUtil
    {
        private CombatGraphView _targetGraphView;
        private CombatContainer _targetContainer;
        private List<Edge> Edges => _targetGraphView.edges.ToList();
        private List<CombatGraphNode> Nodes => _targetGraphView.nodes.ToList().Cast<CombatGraphNode>().ToList();

        public static GraphSaveUtil Create(CombatGraphView targetGraphView)
        {
            return new GraphSaveUtil { _targetGraphView = targetGraphView };
        }

        public void SaveGraph(string fileName)
        {
            var container = ScriptableObject.CreateInstance<CombatContainer>();
            if (!SaveNodes(container))
                return;
            SaveExposedProperties(container);

            EnsureFolderExist();
            AssetDatabase.CreateAsset(container, $"Assets/Res/Graph/Combat/{fileName}.asset");
            AssetDatabase.SaveAssets();
        }

        private void SaveExposedProperties(CombatContainer container)
        {
            container.ExposedProperties.AddRange(_targetGraphView.ExposedProperties);
        }

        private bool SaveNodes(CombatContainer container)
        {
            if (!Edges.Any())
                return false;

            var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
            for (int i = 0; i < connectedPorts.Length; i++)
            {
                var outputNode = connectedPorts[i].output.node as CombatGraphNode;
                var inputNode = connectedPorts[i].input.node as CombatGraphNode;

                container.NodeLinks.Add(new CombatNodeLinkData()
                {
                    baseNodeGuid = outputNode.Guid,
                    portName = connectedPorts[i].output.portName,
                    targetNodeGuid = inputNode.Guid
                });
            }

            foreach (var combatNode in Nodes.Where(node => !node.EntryPoint))
            {
                container.NodeData.Add(new CombatNodeData()
                {
                    guid = combatNode.Guid,
                    text = combatNode.NodeText,
                    position = combatNode.GetPosition().position
                });
            }
            return true;
        }

        public void LoadGraph(string fileName)
        {
            _targetContainer =
                AssetDatabase.LoadAssetAtPath<CombatContainer>($"Assets/Res/Graph/Combat/{fileName}.asset");
            if (_targetContainer == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target graph file does not exist!", "OK");
                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
            CreateExposedProperties();
        }

        private void CreateExposedProperties()
        {
            _targetGraphView.ClearBlackBoardAndExposedProperties();
            foreach (var property in _targetContainer.ExposedProperties)
            { 
                _targetGraphView.AddPropertyToBlackboard(property);
            }
        }

        private void ClearGraph()
        {
            Nodes.Find(x => x.EntryPoint).Guid = _targetContainer.NodeLinks[0].baseNodeGuid;
            foreach (var node in Nodes)
            {
                if (node.EntryPoint)
                    continue;
                Edges.Where(x => x.input.node == node)
                     .ToList()
                     .ForEach(edge =>
                     {
                         _targetGraphView.RemoveElement(edge);
                     });
                _targetGraphView.RemoveElement(node);
            }
        }

        private void CreateNodes()
        {
            foreach (var nodeData in _targetContainer.NodeData)
            {
                var tempNode = _targetGraphView.CreateCombatNode(nodeData.text, Vector2.zero);
                tempNode.Guid = nodeData.guid;
                _targetGraphView.AddElement(tempNode);

                var nodePorts = _targetContainer.NodeLinks.Where(x => x.baseNodeGuid == nodeData.guid).ToList();
                nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.portName));
            }
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                var connections = _targetContainer.NodeLinks.Where(x => x.baseNodeGuid == Nodes[i].Guid).ToList();
                for (int j = 0; j < connections.Count; j++)
                {
                    var targetNodeGuid = connections[j].targetNodeGuid;
                    var targetNode = Nodes.First(x => x.Guid == targetNodeGuid);
                    LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
                    targetNode.SetPosition(
                        new Rect(_targetContainer.NodeData.First(x => x.guid == targetNodeGuid).position,
                                 _targetGraphView.DefaultNodeSize));
                }
            }
        }

        private void LinkNodes(Port output, Port input)
        {
            var tempEdge = new Edge() { output = output, input = input, };
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            _targetGraphView.Add(tempEdge);
        }

        private void EnsureFolderExist()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Res"))
                AssetDatabase.CreateFolder("Assets", "Res");
            if (!AssetDatabase.IsValidFolder("Assets/Res/Graph"))
                AssetDatabase.CreateFolder("Assets/Res", "Graph");
            if (!AssetDatabase.IsValidFolder("Assets/Res/Graph/Combat"))
                AssetDatabase.CreateFolder("Assets/Res/Graph", "Combat");
        }
    }

}