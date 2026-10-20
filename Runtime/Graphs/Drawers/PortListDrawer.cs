using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(IList), 90, true)]
    public class PortListDrawer : NodeFieldDrawer
    {
        public override bool IsValid()
        {
            return HasAttribute<PortFieldAttribute>();
        }

        private IList _localList;
        
        public override VisualElement Build()
        {
            _localList ??= GenerateList();
            
            VisualElement root = new VisualElement();
            
            for (var i = 0; i < _localList.Count; i++)
            {
                var drawer = CreateIndexedDrawer(_localList, i);
                root.Add(drawer.Build());
                _indexedDrawers.Add(drawer);
            }

            return root;
        }

        private void RefreshArray()
        {
            if (!FieldType.IsArray) return;
            Array array = Array.CreateInstance(ElementType, _localList.Count);
            _localList.CopyTo(array, 0);
            SetValueWithoutNotify(array);
        }

        public VisualElement OnAdd()
        {
            _localList.Add(null);
            RefreshArray();
            return CreateIndexedDrawer(_localList, _localList.Count - 1).Build();
        }

        public void OnRemove(int index)
        {
            _localList.RemoveAt(index);
            for (int i = index; i < _localList.Count; i++)
            {
                _indexedDrawers[i].SetIndex(i);
            }
            RefreshArray();
        }
        
        private IList GenerateList()
        {
            IList listValue = GetValue<IList>();
            
            Type listType = typeof(List<>).MakeGenericType(ElementType);
            IList localList = (IList)Activator.CreateInstance(listType);
            if(FieldType.IsArray)
            {
                if (listValue != null)
                {
                    foreach (object value in listValue)
                    {
                        localList.Add(value);
                    }
                }
            }
            else
            {
                if (listValue != null)
                {
                    foreach (var value in listValue)
                    {
                        localList.Add(value);
                    }
                }
                SetValueWithoutNotify(localList);
            }

            return localList;
        }
    }
}