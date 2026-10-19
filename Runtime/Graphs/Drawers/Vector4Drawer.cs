using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(Vector4), 100)]
    public class Vector4Drawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new Vector4Field($"{Name}");
            
            field.SetValueWithoutNotify(GetValue<Vector4>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}