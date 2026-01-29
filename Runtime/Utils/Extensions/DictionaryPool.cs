using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace IRG
{
    public class DictionaryPool<TKey, TValue> where TValue : class
    {
        private readonly Func<TKey, TValue> _createFunc;
        private readonly Action<TValue> _actionOnGet;
        private readonly Action<TValue> _actionOnRelease;
        private readonly Action<TValue> _actionOnDestroy;
        private readonly bool _collectionCheck;
        private readonly int _defaultCapacity;
        private readonly int _maxSize;
        
        private readonly Dictionary<TKey, ObjectPool<TValue>> _dictionary = new();
        
        public DictionaryPool(Func<TKey, TValue> createFunc, Action<TValue> actionOnGet = null,
            Action<TValue> actionOnRelease = null, Action<TValue> actionOnDestroy = null,
            bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            _createFunc = createFunc;
            _actionOnGet = actionOnGet;
            _actionOnRelease = actionOnRelease;
            _actionOnDestroy = actionOnDestroy;
            _collectionCheck = collectionCheck;
            _defaultCapacity = defaultCapacity;
            _maxSize = maxSize;
        }

        public TValue Get(TKey key)
        {
            TValue result;
            if (_dictionary.TryGetValue(key, out var objectPool))
            {
                result = objectPool.Get();
            }
            else
            {
                _dictionary[key] = new ObjectPool<TValue>(() => _createFunc(key), _actionOnGet,  _actionOnRelease, _actionOnDestroy, _collectionCheck,  _defaultCapacity, _maxSize);
                result = _dictionary[key].Get();
            }

            return result;
        }

        public void Release(TKey key, TValue value)
        {
            if (!_dictionary.TryGetValue(key, out var objectPool)) throw new KeyNotFoundException("[DictionaryPool] Trying to release an object without using Get first.]");
            objectPool.Release(value);
        }

        public void Clear()
        {
            foreach (var pool in _dictionary.Values)
            {
                pool.Clear();
            }
            _dictionary.Clear();
        }
    }
}
