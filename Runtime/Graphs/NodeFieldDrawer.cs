using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

namespace IRG.Graphs
{
    public abstract class NodeFieldDrawer
    {
        private FieldInfo _field;
        private object _data;

        private IList _list;
        private int _index;

        public bool IsIndexed => _list != null; 
        public bool IsReadOnly => _field.IsDefined(typeof(IsReadOnlyAttribute));
        public bool IsDebug => _field.IsDefined(typeof(OnlyDebugAttribute));
        
        public string Name => IsIndexed ? $"Element {_index}" : _field.Name.WithSpacesUnderscore();
        public Type FieldType => IsIndexed ? ElementType : _field.FieldType;

        public Type ElementType
        {
            get
            {
                if (IsIndexed)
                {
                    return _list.GetType().GenericTypeArguments[0];
                }

                if (_field.FieldType.IsArray)
                {
                    return _field.FieldType.GetElementType();
                }

                return _field.FieldType.GenericTypeArguments[0];
            }
        }
        
        public event Action OnBeforeValueChanged;
        public event Action OnValueChanged;
        
        public void Initialize(FieldInfo fieldInfo, object data)
        {
            _field = fieldInfo;
            _data = data;
        }
        
        public void Initialize(FieldInfo fieldInfo, object data, IList list, int index)
        {
            Initialize(fieldInfo, data);
            _list = list;
            _index = index;
        }
        
        protected T GetValue<T>() => (T)GetValue();
        protected object GetValue()
        {
            return IsIndexed ? _list[_index] : _field.GetValue(_data);
        }
        
        protected void SetValue(object value)
        {
            OnBeforeValueChanged?.Invoke();
            if (IsIndexed) _list[_index] = value;
            else _field.SetValue(_data, value);
            OnValueChanged?.Invoke();
        }
        
        public virtual bool IsValid() => true;
        public abstract VisualElement Build();

        protected NodeFieldDrawer GetIndexedDrawer(IList list, int index)
        {
            return NodeFieldDrawerFactory.GetIndexedDrawer(_field, _data, list, index);
        }
    }
}