using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG
{
    [UxmlElement]
    public partial class SearchList : ListView
    {
        [UxmlAttribute("header-title")]
        public string HeaderTitle
        {
            get => headerTitle;
            set
            {
                headerTitle = value;
                _titleLabel.text = headerTitle;
            }
        }
        
        [UxmlAttribute("allow-add")]
        public bool AllowAdd
        {
            get => allowAdd;
            set
            {
                allowAdd = value;
                _addButton.SetDisplay(allowAdd);
            }
        }
        
        [UxmlAttribute("allow-remove")]
        public bool AllowRemove
        {
            get => allowRemove;
            set
            {
                allowRemove = value;
                _removeButton.SetDisplay(allowRemove);
            }
        }

        private VisualElement _headerRoot;
        private Label _titleLabel;
        private Button _addButton;
        private Button _removeButton;
        private TextField _searchField;
        private Button _clearButton;

        public event Action OnAdd;
        public event Action OnRemove;
        public event Action<string> OnSearch;

        public SearchList()
        {
            makeHeader = () =>
            {
                var visualAsset = Resources.Load<VisualTreeAsset>("SearchList");
                _headerRoot = visualAsset.Instantiate();
                
                _titleLabel = _headerRoot.Q<Label>("Title");
            
                _addButton = _headerRoot.Q<Button>("AddButton");
                _addButton.clicked += OnAddItem;
                onAdd = _ => OnAddItem();
            
                _removeButton = _headerRoot.Q<Button>("RemoveButton");
                _removeButton.clicked += OnRemoveItem;
                onRemove = _ => OnRemoveItem();
            
                _searchField = _headerRoot.Q<TextField>("SearchField");
                _searchField.RegisterValueChangedCallback(_ => Rebuild());
            
                _clearButton = _headerRoot.Q<Button>("ClearButton");
                _clearButton.clicked += OnClear;
                
                return _headerRoot;
            };


            schedule.Execute(() => showAddRemoveFooter = false);
        }

        public void SetSearchWithoutNotify(string search)
        {
            _searchField.SetValueWithoutNotify(search);
        }

        private void OnAddItem()
        {
            OnAdd?.Invoke();
        }

        private void OnRemoveItem()
        {
            OnRemove?.Invoke();
        }

        private void OnClear()
        {
            _searchField.value = "";
        }

        public new void Rebuild()
        {
            OnSearch?.Invoke(_searchField.value);
            base.Rebuild();
        }
    }
}