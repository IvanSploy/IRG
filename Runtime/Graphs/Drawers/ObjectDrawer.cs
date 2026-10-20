using System;
using System.Collections;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(object), 101, true)]
    public class ObjectDrawer : NodeFieldDrawer
    {
        private object _objData;
        
        public override bool IsValid()
        {
            return !typeof(IList).IsAssignableFrom(FieldType) && !FieldType.IsGenericType && !FieldType.IsEnum;
        }

        public override VisualElement Build()
        {
            _objData = GetValue();
            if (_objData == null)
            {
                _objData = Activator.CreateInstance(FieldType);
                SetValueWithoutNotify(_objData);
            }
            
            var foldout = new Foldout { text = Name };

            var drawers = NodeFieldDrawerFactory.CreateDrawers(_objData);
            foreach (var drawer in drawers)
            {
                foldout.Add(drawer.Build());
                if (FieldType.IsValueType)
                {
                    drawer.OnValueChanged += () => SetValue(_objData);;
                }
                AddDrawerForEvents(drawer);
            }
            
            return foldout;
        }
    }
}