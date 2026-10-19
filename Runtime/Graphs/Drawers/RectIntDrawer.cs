using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(RectInt), 100)]
    public class RectIntDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new RectIntField($"{Name}");
            
            field.SetValueWithoutNotify(GetValue<RectInt>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}