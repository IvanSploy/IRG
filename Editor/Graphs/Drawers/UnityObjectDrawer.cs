using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(Object), 100, true)]
    public class UnityObjectDrawer : NodeFieldDrawer
    {
        public override VisualElement Build()
        {
            ObjectField field = new ObjectField(Name);
            field.objectType = FieldType;
            
            field.SetValueWithoutNotify((Object)GetValue());
            field.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });
            return field;
        }
    }
}