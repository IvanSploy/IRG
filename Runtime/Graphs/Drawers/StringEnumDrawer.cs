using System;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(string), 90)]
    public class StringEnumDrawer : NodeFieldDrawer
    {
        public override bool IsValid()
        {
            return HasAttribute<EnumAttribute>();
        }

        public override VisualElement Build()
        {
            var enumAttribute = GetAttribute<EnumAttribute>();
            var field = new EnumField($"{Name}");

            string stringValue = GetValue<string>();
            if (!Enum.TryParse(enumAttribute.EnumType, stringValue, out object parsedEnum))
            {
                parsedEnum = Activator.CreateInstance(enumAttribute.EnumType);
                SetValueWithoutNotify(Enum.GetName(enumAttribute.EnumType, parsedEnum));
            }
            field.Init((Enum)parsedEnum);
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(Enum.GetName(enumAttribute.EnumType, evt.newValue));
            });

            return field;
        }
    }
}