using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(Rect), 100)]
    public class RectDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new RectField($"{Name}");
            
            field.SetValueWithoutNotify(GetValue<Rect>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}