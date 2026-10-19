using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(int), 100)]
    public class IntDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new IntegerField($"{Name}")
            {
                isDelayed = true,
            };
            if (IsReadOnly) field.isReadOnly = true;
            
            field.SetValueWithoutNotify(GetValue<int>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}