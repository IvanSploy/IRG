using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(double), 100)]
    public class DoubleDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new DoubleField($"{Name}")
            {
                isDelayed = true,
            };
            if (IsReadOnly) field.isReadOnly = true;
            
            field.SetValueWithoutNotify(GetValue<double>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}