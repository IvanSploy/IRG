using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(string), 100)]
    public class StringDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new TextField($"{Name}")
            {
                isDelayed = true,
            };
            if (IsReadOnly) field.isReadOnly = true;
            if (FieldType.IsDefined(typeof(TextAreaAttribute))) field.multiline = true;
            
            field.SetValueWithoutNotify(GetValue<string>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}