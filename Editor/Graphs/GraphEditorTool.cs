using System.Collections.Generic;
using IRG;
using IRG.Editor;
using IRG.Graphs;
using IRG.Graphs.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class GraphEditorTool : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;

        private Toolbar _toolbar;
        private LabelEditable _title;
        private Button _refreshButton;
        private Button _saveButton;
        private Toggle _autoSaveToggle;
        private Toggle _showHistoryButton;
        private Graph _graph;
        private GraphData _currentData;

        private static readonly Dictionary<GraphData, GraphEditorTool> _openedWindows = new();
        private static bool _autoSave;
        private static bool _showHistory;
        
        private static void Open()
        {
            var window = GetWindow<GraphEditorTool>();
            window.titleContent = new GUIContent("Graph Tool");
        }
        
        public static void Open(GraphData data)
        {
            int autoSave = PlayerPrefs.GetInt("GraphTool_AutoSave");
            _autoSave = autoSave == 1;
            
            int showHistory = PlayerPrefs.GetInt("GraphTool_ShowHistory");
            _showHistory = showHistory == 1;
            
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
            
            _toolbar = rootVisualElement.Q<Toolbar>();
            _title = _toolbar.Q<LabelEditable>("Title");
            _title.RegisterValueChangedCallback(evt =>
            {
                RenameCurrent(evt.newValue);
            });
            
            _refreshButton = _toolbar.Q<Button>("Refresh");
            _refreshButton.clicked += Refresh;
            
            _saveButton = _toolbar.Q<Button>("Save");
            _saveButton.clicked += Save;
            
            _autoSaveToggle = _toolbar.Q<Toggle>("AutoSave");
            _autoSaveToggle.RegisterValueChangedCallback(evt =>
            {
                _autoSave = evt.newValue;
                PlayerPrefs.SetInt("GraphTool_AutoSave", _autoSave ? 1 : 0);
            });
            
            _showHistoryButton = _toolbar.Q<Toggle>("ShowHistory");
            _showHistoryButton.RegisterValueChangedCallback(evt =>
            {
                _graph.ShowUndoRedoHistory = _showHistory = evt.newValue;
                PlayerPrefs.SetInt("GraphTool_ShowHistory", _showHistory ? 1 : 0);
            });
            
            _graph = rootVisualElement.Q<Graph>();
            _graph.ShowUndoRedoHistory = _showHistory;
            _graph.OnDirtyRefresh += value =>
            {
                _saveButton.text = value ? "Save *" : "Save";
            };
            
            Load();
            
            rootVisualElement.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (!evt.ctrlKey) return;

                bool performed = false;
                switch (evt.keyCode)
                {
                    case KeyCode.S:
                        performed = true;
                        Save();
                        break;
                }
                if(performed) evt.StopPropagation();
            });
        }

        private void OnDisable()
        {
            if (!_currentData) return;
            _openedWindows.Remove(_currentData);
            AskForSave();
        }
        
        private void Refresh()
        {
            _graph.Refresh();
        }
        
        private void Load()
        {
            if (!_currentData) return;
            _graph.Load(_currentData);
            _title.SetValueWithoutNotify(_currentData.name);
        }

        private void Save()
        {
            if (!_currentData) return;
            _graph.Save(_currentData);
        }
        
        private void AskForSave()
        {
            if (!_currentData) return;
            if (!_graph.IsDirty) return;
            if (_autoSave || EditorUtility.DisplayDialog("Graph not saved", 
                    "Do you want to save the changes made in the current graph?",
                    "Yes",
                    "No"))
            {
                _graph.Save(_currentData);
            }
        }

        private void RenameCurrent(string fileName)
        {
            if(!_currentData) return;
            if (string.IsNullOrEmpty(fileName)) return;

            AskForSave();
            AssetsIO.RenameAsset(_currentData, fileName);
        }
    }
}