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
            var field = new TextField()
            {
                isDelayed = true,
            };
            if (!HasAttribute<NoLabelAttribute>()) field.label = Name;
            if (IsReadOnly) field.isReadOnly = true;
            if (HasAttribute<TextAreaAttribute>())
            {
                field.multiline = true;

                var label = field.Q<Label>();
                if (label != null) label.style.minWidth = 70f;
                
                var textElement = field.Query<TextElement>().Last();
                textElement.style.flexGrow = 0f;
                textElement.style.flexShrink = 0f;
                textElement.style.unityTextAlign = TextAnchor.MiddleLeft;
                textElement.RegisterCallback<GeometryChangedEvent>(evt =>
                {
                    if (label != null) field.style.height = textElement.resolvedStyle.height + label.resolvedStyle.height;
                    else field.style.height = textElement.resolvedStyle.height;
                });
                textElement.style.paddingBottom = 5f;
            }
            
            field.SetValueWithoutNotify(GetValue<string>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}