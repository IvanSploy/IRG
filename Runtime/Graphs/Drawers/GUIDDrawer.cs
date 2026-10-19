using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(GUID), 100)]
    public class GUIDDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new GUIDField($"{Name}");
            if (IsReadOnly) field.isReadOnly = true;
            
            field.SetValueWithoutNotify(GetValue<GUID>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}