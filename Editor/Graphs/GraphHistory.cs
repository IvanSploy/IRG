using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Editor
{
    [UxmlElement]
    public partial class GraphHistory : VisualElement
    {
        private ListView _redoList;
        private ListView _undoList;
        
        public List<string> RedoSource { private get; set; }
        public List<string> UndoSource { private get; set; }

        public event Action<int> OnUndo;
        public event Action<int> OnRedo;

        public GraphHistory()
        {
            style.backgroundColor = Color.gray2;
            _redoList = new ListView
            {
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                selectionType = SelectionType.None
            };
            Add(_redoList);
            
            _redoList.style.borderLeftWidth = 5f;
            _redoList.style.borderLeftColor = Color.red;
            _redoList.makeItem = () =>
            {
                var button = new Button();
                button.clicked += () =>
                {
                    var index = (int)button.userData;
                    OnRedo?.Invoke(index);
                };
                return new Button();
            };
            _redoList.bindItem = (ve, i) =>
            {
                var button = (Button)ve;
                button.text = RedoSource[i];
            };
            
            _undoList = new ListView()
            {
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                selectionType = SelectionType.None
            };
            Add(_undoList);
            
            _undoList.style.borderTopWidth = 5f;
            _undoList.style.borderTopColor = Color.ghostWhite;
            _undoList.makeItem = () =>
            {
                var button = new Button();
                button.clicked += () =>
                {
                    var index = (int)button.userData;
                    OnRedo?.Invoke(index);
                };
                return new Button();
            };
            _undoList.bindItem = (ve, i) =>
            {
                var button = (Button)ve;
                button.text = UndoSource[i];
            };
            
            _redoList.SetDisplay(false);
            _undoList.SetDisplay(false);
        }

        public void Rebuild()
        {
            _redoList.itemsSource = RedoSource;
            if(RedoSource is { Count: > 0 })
            {
                _redoList.SetDisplay(true);
                _redoList.Rebuild();
            }
            else _redoList.SetDisplay(false);
            
            _undoList.itemsSource = UndoSource;
            if (UndoSource is { Count: > 0 })
            {
                _undoList.SetDisplay(true);
                _undoList.Rebuild();
            }
            else _undoList.SetDisplay(false);
        }
    }
}