using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(Vector3), 100)]
    public class Vector3Drawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new Vector3Field($"{Name}");
            
            field.SetValueWithoutNotify(GetValue<Vector3>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}