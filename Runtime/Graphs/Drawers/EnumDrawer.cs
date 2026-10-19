using System;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(Enum))]
    public class EnumDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new EnumField($"{Name}");
            
            field.Init(GetValue<Enum>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}