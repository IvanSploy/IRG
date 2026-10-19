using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(long), 100)]
    public class LongDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new LongField($"{Name}")
            {
                isDelayed = true,
            };
            if (IsReadOnly) field.isReadOnly = true;
            
            field.SetValueWithoutNotify(GetValue<long>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}