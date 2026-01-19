using System;

namespace IRG
{
    public class ReactiveProperty<T>
    {
        private T _value;
        public T Value {
            get => _value;
            set
            {
                if (_value.Equals(value)) return;
                
                var previous = _value;
                _value = value;
                NotifyEvent?.Invoke(_value);
                NotifyPairEvent?.Invoke(previous, _value);
            }
        }
        
        public delegate void ValueChange( T previousValue, T newValue);
        private event Action<T> NotifyEvent;
        private event ValueChange NotifyPairEvent;

        public ReactiveProperty() { }
        
        public ReactiveProperty(T initialValue)
        {
            Value = initialValue;
        }

        public void Subscribe(Action<T> action)
        {
            NotifyEvent += action;
            action?.Invoke(_value);
        }
        
        public void Subscribe(ValueChange action)
        {
            NotifyPairEvent += action;
            action?.Invoke(default, _value);
        }
        
        public void UnSubscribe(Action<T> action)
        {
            NotifyEvent -= action;
        }
        
        public void UnSubscribe(ValueChange action)
        {
            NotifyPairEvent -= action;
        }
    }
}