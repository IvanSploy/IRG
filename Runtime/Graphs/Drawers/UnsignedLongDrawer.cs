using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(ulong), 100)]
    public class UnsignedLongDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new UnsignedLongField($"{Name}")
            {
                isDelayed = true,
            };
            if (IsReadOnly) field.isReadOnly = true;
            
            field.SetValueWithoutNotify(GetValue<ulong>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}