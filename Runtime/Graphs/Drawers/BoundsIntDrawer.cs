using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(BoundsInt), 100)]
    public class BoundsIntDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new BoundsIntField($"{Name}");
            
            field.SetValueWithoutNotify(GetValue<BoundsInt>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}