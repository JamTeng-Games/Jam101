using System.Linq;
using Jam.Core;
using Jam.Runtime.Combat_;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Blackboard = UnityEditor.Experimental.GraphView.Blackboard;

namespace Jam.Editor_.Combat
{

    public class CombatGraph : EditorWindow
    {
        private CombatGraphView _graphView;
        private string _fileName = "New Combat Graph";

        [MenuItem("Window/Combat Graph View")]
        public static CombatGraph OpenWindow()
        {
            var window = GetWindow<CombatGraph>();
            window.titleContent = new GUIContent("Combat Graph View");
            return window;
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMinimap();
            GenerateBlackboard();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }

        private void ConstructGraphView()
        {
            _graphView = new CombatGraphView(this) { name = "Combat graph", };
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();
            // File name
            var fileNameTextField = new TextField("File Name");
            fileNameTextField.SetValueWithoutNotify(_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt =>
            {
                _fileName = evt.newValue;
            });
            toolbar.Add(fileNameTextField);

            // Save button
            var saveButton = new Button(SaveData);
            saveButton.text = "Save Data";
            toolbar.Add(saveButton);

            // Load button
            var loadButton = new Button(LoadData);
            loadButton.text = "Load Data";
            toolbar.Add(loadButton);

            // // Create node button
            // var nodeCreateButton = new Button(() =>
            // {
            //     _graphView.CreateNode("Dialogue Node");
            // });
            // nodeCreateButton.text = "Create Node";
            // toolbar.Add(nodeCreateButton);

            rootVisualElement.Add(toolbar);
        }

        private void GenerateMinimap()
        {
            var miniMap = new MiniMap() { anchored = true, };
            var cords = _graphView.contentViewContainer.WorldToLocal(new Vector2(maxSize.x - 10, 30));
            JLog.Info(cords);
            // miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
            miniMap.SetPosition(new Rect(1000, 30, 200, 140));
            _graphView.Add(miniMap);
        }

        private void GenerateBlackboard()
        {
            var blackboard = new Blackboard(_graphView);
            blackboard.Add(new BlackboardSection { title = "Exposed Properties", });
            blackboard.addItemRequested = _blackboard =>
            {
                _graphView.AddPropertyToBlackboard(new ExposedProperty());
            };
            blackboard.editTextRequested = (blackboard1, element, newValue) =>
            {
                var oldPropertyName = ((BlackboardField)element).text;
                if (_graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
                {
                    EditorUtility.DisplayDialog(
                        "Error", "This property name already exists, please choose another one.", "OK");
                    return;
                }

                var propertyIndex = _graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
                _graphView.ExposedProperties[propertyIndex].PropertyName = newValue;
                var propertyView = (BlackboardField)element;
                propertyView.text = newValue;
            };
            blackboard.SetPosition(new Rect(10, 30, 200, 300));

            _graphView.Add(blackboard);
            _graphView.Blackboard = blackboard;
        }

        private void LoadData()
        {
            if (string.IsNullOrWhiteSpace(_fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
                return;
            }

            var saveUtil = GraphSaveUtil.Create(_graphView);
            saveUtil.LoadGraph(_fileName);
        }

        private void SaveData()
        {
            if (string.IsNullOrWhiteSpace(_fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
                return;
            }

            var saveUtil = GraphSaveUtil.Create(_graphView);
            saveUtil.SaveGraph(_fileName);
        }
    }

}