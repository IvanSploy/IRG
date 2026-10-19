using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(Bounds), 100)]
    public class BoundsDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new BoundsField($"{Name}");
            
            field.SetValueWithoutNotify(GetValue<Bounds>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}