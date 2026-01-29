using System;
using System.Collections.Generic;

namespace IRG
{
    public static class CollectionExtension
    {
        public static void AddToList<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key,
            TValue value)
        {
            if (dictionary.TryGetValue(key, out var list))
            {
                list.Add(value);
            }
            else
            {
                dictionary[key] = new List<TValue> { value };
            }
        }

        public static void RemoveFromList<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key,
            TValue value)
        {
            if (dictionary.TryGetValue(key, out var list))
            {
                list.Remove(value);
                if (list.Count == 0)
                {
                    dictionary.Remove(key);
                }
            }
        }

        public static void AddToSet<TKey, TValue>(this Dictionary<TKey, HashSet<TValue>> dictionary, TKey key,
            TValue value)
        {
            if (dictionary.TryGetValue(key, out var set))
            {
                set.Add(value);
            }
            else
            {
                dictionary[key] = new HashSet<TValue> { value };
            }
        }

        public static void RemoveFromSet<TKey, TValue>(this Dictionary<TKey, HashSet<TValue>> dictionary, TKey key,
            TValue value)
        {
            if (dictionary.TryGetValue(key, out var set))
            {
                set.Remove(value);
                if (set.Count == 0)
                {
                    dictionary.Remove(key);
                }
            }
        }
        
        public static void Push<TKey, TValue>(this Dictionary<TKey, Stack<TValue>> dictionary, TKey key,
            TValue value)
        {
            if (dictionary.TryGetValue(key, out var stack))
            {
                stack.Push(value);
            }
            else
            {
                dictionary[key] = new Stack<TValue>();
                dictionary[key].Push(value);
            }
        }

        public static TValue Pop<TKey, TValue>(this Dictionary<TKey, Stack<TValue>> dictionary, TKey key)
        {
            TValue value = default;
            if (dictionary.TryGetValue(key, out var stack))
            {
                value = stack.Pop();
                if (stack.Count == 0)
                {
                    dictionary.Remove(key);
                }
            }

            return value;
        }
        
        public static void AddRange<TValue>(this HashSet<TValue> set, IEnumerable<TValue> values)
        {
            foreach (var value in values)
            {
                set.Add(value);
            }
        }
        
        public static void AddTo(this IDisposable disposable, ICollection<IDisposable> disposables)
        {
            disposables.Add(disposable);
        }
    }
}
