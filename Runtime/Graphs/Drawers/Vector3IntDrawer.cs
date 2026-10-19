using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(Vector3Int), 100)]
    public class Vector3IntDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new Vector3IntField($"{Name}");
            
            field.SetValueWithoutNotify(GetValue<Vector3Int>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}