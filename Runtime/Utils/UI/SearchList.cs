using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG
{
    [UxmlElement]
    public partial class SearchList : VisualElement
    {
        private string _title;
        [UxmlAttribute] public string Title
        {
            get => _title;
            set
            {
                _title = value;
                TitleLabel.text = _title;
            }
        }
        
        private bool _hasAdd = true;
        [UxmlAttribute] public bool HasAdd
        {
            get => _hasAdd;
            set
            {
                _hasAdd = value;
                AddButton.SetDisplay(_hasRemove);
            }
        }
        
        private bool _hasRemove = true;
        [UxmlAttribute] public bool HasRemove
        {
            get => _hasRemove;
            set
            {
                _hasRemove = value;
                RemoveButton.SetDisplay(_hasRemove);
            } 
        }
        
        private VisualTreeAsset _itemTemplate;
        [UxmlAttribute] public VisualTreeAsset ItemTemplate
        {
            get => _itemTemplate;
            set
            {
                _itemTemplate = value;
                ListView.itemTemplate = _itemTemplate;
            } 
        }
        
        public readonly VisualElement Root;
        public readonly Label TitleLabel;
        public readonly Button AddButton;
        public readonly Button RemoveButton;
        public readonly TextField SearchField;
        public readonly Button ClearButton;
        public readonly ListView ListView;

        public event Action OnAdd;
        public event Action OnRemove;
        public event Action<string> OnSearch;
        
        public SearchList()
        {
            var visualAsset = Resources.Load<VisualTreeAsset>("SearchList");
            Root = visualAsset.Instantiate();
            Add(Root);
            
            TitleLabel = Root.Q<Label>("Title");
            TitleLabel.text = Title;
            
            AddButton = Root.Q<Button>("AddButton");
            AddButton.clicked += OnAddItem;
            
            RemoveButton = Root.Q<Button>("RemoveButton");
            RemoveButton.clicked += OnRemoveItem;
            
            SearchField = Root.Q<TextField>("SearchField");
            SearchField.RegisterValueChangedCallback(_ => Rebuild());
            
            ClearButton = Root.Q<Button>("ClearButton");
            ClearButton.clicked += OnClear;
            
            ListView = Root.Q<ListView>();
            ListView.itemTemplate = _itemTemplate;
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
            SearchField.value = "";
        }

        public void Rebuild()
        {
            OnSearch?.Invoke(SearchField.value);
            ListView.Rebuild();            
        }
        
    }
}