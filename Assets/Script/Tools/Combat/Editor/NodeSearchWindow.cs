using System.Collections.Generic;
using Jam.Core;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jam.Editor_.Combat
{

    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private CombatGraphView _graphView;
        private EditorWindow _editorWindow;
        private Texture2D _indentationIcon;

        public void Init(CombatGraphView graphView, EditorWindow editorWindow)
        {
            _graphView = graphView;
            _editorWindow = editorWindow;
            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            _indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
                new SearchTreeGroupEntry(new GUIContent("Combat Node"), 1),
                new SearchTreeEntry(new GUIContent("Combat Node", _indentationIcon))
                {
                    userData = new CombatGraphNode(), level = 2,
                },
            };
            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var worldMousePos = _editorWindow.rootVisualElement.ChangeCoordinatesTo(
                _editorWindow.rootVisualElement.parent, context.screenMousePosition - _editorWindow.position.position);
            var localMousePos = _graphView.contentViewContainer.WorldToLocal(worldMousePos);

            switch (SearchTreeEntry.userData)
            {
                case CombatGraphNode:
                    JLog.Info("Create Combat Node");
                    _graphView.CreateNode("Combat Node", localMousePos);
                    break;
                default: return false;
            }
            return true;
        }
    }

}