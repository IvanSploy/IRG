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

        public GraphHistory()
        {
            style.backgroundColor = Color.gray2;

            var label = new Label("History");
            label.style.paddingLeft = 10f;
            label.style.paddingRight = 10f;
            label.style.paddingTop = 5f;
            label.style.paddingBottom = 5f;
            label.style.backgroundColor = Color.gray1;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            Add(label);
            
            _redoList = new ListView
            {
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                selectionType = SelectionType.None,
            };
            Add(_redoList);
            
            _redoList.style.borderLeftWidth = 3f;
            _redoList.style.borderLeftColor = Color.red;
            _redoList.makeItem = () =>
            {
                var label = new Label();
                label.style.color = Color.gray;
                label.style.marginLeft = 5f;
                label.style.unityTextAlign = TextAnchor.MiddleLeft;
                label.style.flexGrow = 1f;
                return label;
            };
            _redoList.bindItem = (ve, i) =>
            {
                var label = (Label)ve;
                label.userData = i;
                label.text = RedoSource[i];
            };
            
            _undoList = new ListView()
            {
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                selectionType = SelectionType.None
            };
            Add(_undoList);
            
            _undoList.style.borderTopWidth = 2f;
            _undoList.style.borderTopColor = Color.ghostWhite;
            _undoList.makeItem = () =>
            {
                var label = new Label();
                label.style.marginLeft = 5f;
                label.style.unityTextAlign = TextAnchor.MiddleLeft;
                label.style.flexGrow = 1f;
                return label;
            };
            _undoList.bindItem = (ve, i) =>
            {
                var label = (Label)ve;
                label.userData = i;
                label.text = UndoSource[i];
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