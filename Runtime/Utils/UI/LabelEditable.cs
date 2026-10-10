using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG
{
    [UxmlElement]
    public partial class LabelEditable : VisualElement, INotifyValueChanged<string>
    {
        private string _value;
        
        [UxmlAttribute]
        public string value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                var previous = _value;
                SetValueWithoutNotify(value);
                using (var evt = ChangeEvent<string>.GetPooled(previous, value))
                {
                    SendEvent(evt);
                }
            }
        }

        private readonly Label _label;
        private readonly TextField _textField;
        
        public LabelEditable()
        {
            var visualAsset = Resources.Load<VisualTreeAsset>("LabelEditable");
            var root = visualAsset.Instantiate();
            while (root.childCount > 0)
            {
                var child = root.ElementAt(0);
                Add(child);
            }
            
            _label = this.Q<Label>();
            _label.RegisterValueChangedCallback(evt =>
            {
                evt.StopPropagation();
            });
            
            _textField = this.Q<TextField>();
            _textField.RegisterValueChangedCallback(evt =>
            {
                HideEditor();
                value = evt.newValue;
            });
            
            RegisterCallback<ClickEvent>(_ => ShowEditor());
            RegisterCallback<FocusOutEvent>(_ => HideEditor());
            
            HideEditor();
        }
        
        public void SetValueWithoutNotify(string newValue)
        {
            _value = newValue;
            _label.text = newValue;
            _textField.SetValueWithoutNotify(newValue);
        }

        public void ShowEditor()
        {
            _label.SetDisplay(false);
            _textField.SetDisplay(true);
            _textField.Focus();
        }
        
        public void HideEditor()
        {
            _label.SetDisplay(true);
            _textField.SetDisplay(false);
        }
    }
}