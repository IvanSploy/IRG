using System.Collections.Generic;
using IRG;
using IRG.Graphs;
using IRG.Graphs.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class GraphEditorTool : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;

        private Graph _graph;
        private GraphData _currentData;

        private static readonly Dictionary<GraphData, GraphEditorTool> _openedWindows = new();
        
        private static void Open()
        {
            var window = GetWindow<GraphEditorTool>();
            window.titleContent = new GUIContent("Cinematic Tool");
        }
        
        public static void Open(GraphData data)
        {
            if (!data)
            {
                Open();
                return;
            }

            if (_openedWindows.TryGetValue(data, out GraphEditorTool window))
            {
                window.Focus();
                return;
            }
            
            window = CreateInstance<GraphEditorTool>();
            window.titleContent = new GUIContent($"{data.name} Graph");
            window._currentData = data;
            window.Show();
            _openedWindows.Add(data, window);
        }

        private void CreateGUI()
        {
            _visualTreeAsset.AddTo(rootVisualElement);
            _graph = rootVisualElement.Q<Graph>();
            if(_currentData) Load();
        }

        private void Load()
        {
            _graph.Load(_currentData);
        }

        private void OnDisable()
        {
            if (!_currentData) return;
            _openedWindows.Remove(_currentData);
            _graph.Save(_currentData);
        }
    }
}