using System;
using System.Collections;
using System.Collections.Generic;
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
        
        protected internal readonly List<NodeFieldDrawer> _indexedDrawers = new();
        protected internal void SetIndex(int index) => _index = index;

        public bool IsIndexed => _list != null;
        public bool IsReadOnly => _field.IsDefined(typeof(IsReadOnlyAttribute));
        public bool IsDebug => _field.IsDefined(typeof(OnlyDebugAttribute));

        public bool HasAttribute<T>() where T : Attribute
        {
            return _field.IsDefined(typeof(T));
        }
        
        public T GetAttribute<T>() where T : Attribute
        {
            return _field.GetCustomAttribute<T>();
        }
        
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

        protected void AddDrawerForEvents(NodeFieldDrawer drawer)
        {
            drawer.OnBeforeValueChanged += OnBeforeValueChanged;
            drawer.OnValueChanged += OnValueChanged;
        }
        
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
        
        protected void SetValueWithoutNotify(object value)
        {
            if (IsIndexed) _list[_index] = value;
            else _field.SetValue(_data, value);
        }
        
        public virtual bool IsValid() => true;
        public abstract VisualElement Build();

        protected NodeFieldDrawer CreateIndexedDrawer(IList list, int index)
        {
            var drawer = NodeFieldDrawerFactory.CreateIndexedDrawer(_field, _data, list, index);
            AddDrawerForEvents(drawer);
            _indexedDrawers.Add(drawer);
            return drawer;
        }
    }
}