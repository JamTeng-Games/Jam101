using System.Collections.Generic;
using System.Linq;
using Jam.Runtime.Combat_;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jam.Editor_.Combat
{

    public class CombatGraphView : GraphView
    {
        public Vector2 DefaultNodeSize = new Vector2(150, 200);
        private EditorWindow _window;

        private NodeSearchWindow _searchWindow;

        public Blackboard Blackboard;
        public List<ExposedProperty> ExposedProperties { get; private set; } = new List<ExposedProperty>();

        public CombatGraphView(EditorWindow window)
        {
            _window = window;
            styleSheets.Add(Resources.Load<StyleSheet>("CombatGraph"));
            SetupZoom(0.25f, 2);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddElement(GenerateEntryPoint());
            AddSearchWindow();
        }

        private void AddSearchWindow()
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            _searchWindow.Init(this, _window);
            nodeCreationRequest = ctx =>
                SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), _searchWindow);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            });
            return compatiblePorts;
        }

        public void CreateNode(string nodeName, Vector2 position)
        {
            AddElement(CreateCombatNode(nodeName, position));
        }

        public CombatGraphNode CreateCombatNode(string nodeName, Vector2 position)
        {
            var node = new CombatGraphNode()
            {
                title = nodeName, Guid = System.Guid.NewGuid().ToString(), NodeText = nodeName,
            };

            node.SetPosition(new Rect(position, DefaultNodeSize));

            Port inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            node.inputContainer.Add(inputPort);

            node.styleSheets.Add(Resources.Load<StyleSheet>("CombatNode"));

            // Port outputPort = GeneratePort(node, Direction.Output);
            // outputPort.portName = "Output";
            // node.outputContainer.Add(outputPort);

            var button = new Button(() =>
            {
                AddChoicePort(node);
            });
            button.text = "New Choice";
            node.titleContainer.Add(button);

            var textField = new TextField(string.Empty);
            textField.RegisterValueChangedCallback(evt =>
            {
                node.NodeText = evt.newValue;
                node.title = evt.newValue;
            });
            textField.SetValueWithoutNotify(node.title);
            node.mainContainer.Add(textField);

            RefreshNode(node);

            return node;
        }

        public void AddChoicePort(CombatGraphNode node, string overridePortName = "")
        {
            var newPort = GeneratePort(node, Direction.Output);

            var oldLabel = newPort.contentContainer.Q<Label>("type");
            newPort.contentContainer.Remove(oldLabel);

            var outputPortCount = node.outputContainer.Query("connector").ToList().Count;

            var choicePortName =
                string.IsNullOrEmpty(overridePortName) ? $"Option {outputPortCount + 1}" : overridePortName;

            // var outputPortName = $"Option {outputPortCount + 1}";

            var textField = new TextField() { name = string.Empty, value = overridePortName };
            textField.RegisterValueChangedCallback(evt =>
            {
                newPort.portName = evt.newValue;
            });
            textField.style.minWidth = 60;
            textField.style.maxWidth = 150;
            newPort.contentContainer.Add(new Label("    "));
            newPort.contentContainer.Add(textField);
            var deleteButton = new Button(() => RemovePort(node, newPort)) { text = "X" };
            newPort.contentContainer.Add(deleteButton);

            newPort.portName = choicePortName;
            node.outputContainer.Add(newPort);
            RefreshNode(node);
        }

        private void RemovePort(CombatGraphNode node, Port newPort)
        {
            // Get edge
            var targetEdge = edges.ToList()
                                  .Where(x => x.output.portName == newPort.portName && x.output.node == newPort.node);
            if (targetEdge.Any())
            {
                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());
            }
            node.outputContainer.Remove(newPort);
            RefreshNode(node);
        }

        private CombatGraphNode GenerateEntryPoint()
        {
            var node = new CombatGraphNode()
            {
                title = "START", Guid = System.Guid.NewGuid().ToString(), NodeText = "ENTRYPOINT", EntryPoint = true,
            };

            node.SetPosition(new Rect(100f, 200f, 100f, 150f));

            Port port = GeneratePort(node, Direction.Output);
            port.portName = "NEXT";
            node.outputContainer.Add(port);

            node.capabilities &= ~Capabilities.Movable;
            node.capabilities &= ~Capabilities.Deletable;

            RefreshNode(node);

            return node;
        }

        private Port GeneratePort(CombatGraphNode node,
                                  Direction portDirection,
                                  Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
        }

        public static void RefreshNode(Node node)
        {
            node.RefreshExpandedState();
            node.RefreshPorts();
        }

        public void ClearBlackBoardAndExposedProperties()
        {
            ExposedProperties.Clear();
            Blackboard.Clear();
        }

        public void AddPropertyToBlackboard(ExposedProperty inProperty)
        {
            var localPropertyName = inProperty.PropertyName;
            var localPropertyValue = inProperty.PropertyValue;
            while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
            {
                localPropertyName = $"{localPropertyName}(1)";
            }

            var property = new ExposedProperty();
            property.PropertyName = localPropertyName;
            property.PropertyValue = localPropertyValue;
            ExposedProperties.Add(property);

            var container = new VisualElement();
            var blackboardField = new BlackboardField() { text = property.PropertyName, typeText = "string" };
            container.Add(blackboardField);

            var propertyValueTextField = new TextField("Value") { value = localPropertyValue };
            propertyValueTextField.RegisterValueChangedCallback(evt =>
            {
                var changingPropertyIndex = ExposedProperties.FindIndex(x => x.PropertyName == property.PropertyName);
                ExposedProperties[changingPropertyIndex].PropertyValue = evt.newValue;
            });
            var blackboardValueRow = new BlackboardRow(blackboardField, propertyValueTextField);
            container.Add(blackboardValueRow);

            Blackboard.Add(container);
        }
    }

}