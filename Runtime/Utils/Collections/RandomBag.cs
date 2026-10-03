using System;
using System.Collections.Generic;

namespace IRG.Collections
{
    public class RandomBag<T>
    {
        private readonly List<T> _items = new();

        public int Count => _items.Count;
        
        public void Add(T item) => _items.Add(item);
        public void AddRange(IEnumerable<T> items) => _items.AddRange(items);

        public T Draw()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Bag is empty.");

            int index = UnityEngine.Random.Range(0, _items.Count);
            T item = _items[index];

            _items[index] = _items.Last();
            _items.RemoveLast();

            return item;
        }
    }
}