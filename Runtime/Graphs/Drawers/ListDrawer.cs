using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(IList), 100, true)]
    public class ListDrawer : NodeFieldDrawer
    {
        private IList _localList;
        
        public override VisualElement Build()
        {
            IList listValue = GetValue<IList>();
            
            Type listType = typeof(List<>).MakeGenericType(ElementType);
            _localList = (IList)Activator.CreateInstance(listType);
            if(FieldType.IsArray)
            {
                if (listValue != null)
                {
                    foreach (object value in listValue)
                    {
                        _localList.Add(value);
                    }
                }
            }
            else
            {
                if (listValue != null)
                {
                    foreach (var value in listValue)
                    {
                        _localList.Add(value);
                    }
                }
                SetValueWithoutNotify(_localList);
            }
            
            var field = new ListView()
            {
                showFoldoutHeader = true,
                headerTitle = Name,
                allowAdd = true,
                allowRemove = true,
                showAddRemoveFooter = true,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly,
            };
            
            field.itemsSource = _localList;
            field.makeItem = () => new VisualElement();
            field.bindItem = (element, i) =>
            {
                var drawer = CreateIndexedDrawer(_localList, i);
                element.Add(drawer.Build());
            };
            field.unbindItem = (element, i) =>
            {
                element.Clear();
            };
            
            field.Rebuild();
            
            OnValueChanged += () =>
            {
                if (!FieldType.IsArray) return;
                Array array = Array.CreateInstance(ElementType, _localList.Count);
                _localList.CopyTo(array, 0);
                SetValue(array);
            };

            return field;
        }
    }
}