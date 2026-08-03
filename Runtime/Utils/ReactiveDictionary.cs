using System;
using System.Collections.Generic;

namespace IRG
{
    public class ReactiveDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new();
        
        public delegate void KeyChange(TKey key, TValue value);
        public delegate void ValueChange(TKey key, TValue previousValue, TValue newValue);
        private event KeyChange NotifyAddEvent;
        private event ValueChange NotifyUpdateEvent;
        private event KeyChange NotifyRemoveEvent;

        public IDisposable SubscribeAdd(KeyChange action)
        {
            NotifyAddEvent += action;
            return new ActionDisposable(() => UnSubscribeAdd(action));
        }
        
        public void UnSubscribeAdd(KeyChange action)
        {
            NotifyAddEvent -= action;
        }
        
        public IDisposable SubscribeUpdate(ValueChange action)
        {
            NotifyUpdateEvent += action;
            return new ActionDisposable(() => UnSubscribeUpdate(action));
        }
        
        public void UnSubscribeUpdate(ValueChange action)
        {
            NotifyUpdateEvent -= action;
        }
        
        public IDisposable SubscribeRemove(KeyChange action)
        {
            NotifyRemoveEvent += action;
            return new ActionDisposable(() => UnSubscribeRemove(action));
        }
        
        public void UnSubscribeRemove(KeyChange action)
        {
            NotifyRemoveEvent -= action;
        }
        
        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                if (_dictionary.ContainsKey(key))
                {
                    var oldValue = _dictionary[key];
                    _dictionary[key] = value;
                    NotifyUpdateEvent?.Invoke(key, oldValue, value);
                }
                else
                {
                    _dictionary[key] = value;
                    NotifyAddEvent?.Invoke(key, value);
                }
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }
        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            NotifyAddEvent?.Invoke(key, value);
        }
        public bool TryAdd(TKey key, TValue value)
        {
            if (_dictionary.ContainsKey(key)) return false;
            NotifyAddEvent?.Invoke(key, value);
            return true;
        }
        public bool Remove(TKey key) => Remove(key, out _);
        public bool Remove(TKey key, out TValue value)
        {
            var removed = _dictionary.Remove(key, out value);
            if (!removed) return false;
            NotifyRemoveEvent?.Invoke(key, value);
            return true;
        }
    }
}