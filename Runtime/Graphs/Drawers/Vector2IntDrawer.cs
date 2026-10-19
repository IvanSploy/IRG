using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(Vector2Int), 100)]
    public class Vector2IntDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            var field = new Vector2IntField($"{Name}");
            
            field.SetValueWithoutNotify(GetValue<Vector2Int>());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });

            return field;
        }
    }
}