using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(Vector2), 100)]
    public class Vector2Drawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new Vector2Field($"{Name}");
            
            field.SetValueWithoutNotify(GetValue<Vector2>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}