using System.Collections.Generic;

public static class DictionaryExtension
{
    public static void AddToList<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
    {
        if (dictionary.TryGetValue(key, out var collection))
        {
            collection.Add(value);
        }
        else
        {
            dictionary[key] = new List<TValue> {value};
        }
    }
    
    public static void RemoveFromList<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
    {
        if (dictionary.TryGetValue(key, out var collection))
        {
            collection.Remove(value);
            if (collection.Count == 0)
            {
                dictionary.Remove(key);
            }
        }
    }
    
    public static void AddToSet<TKey, TValue>(this Dictionary<TKey, HashSet<TValue>> dictionary, TKey key, TValue value)
    {
        if (dictionary.TryGetValue(key, out var collection))
        {
            collection.Add(value);
        }
        else
        {
            dictionary[key] = new HashSet<TValue> {value};
        }
    }
    
    public static void RemoveFromSet<TKey, TValue>(this Dictionary<TKey, HashSet<TValue>> dictionary, TKey key, TValue value)
    {
        if (dictionary.TryGetValue(key, out var collection))
        {
            collection.Remove(value);
            if (collection.Count == 0)
            {
                dictionary.Remove(key);
            }
        }
    }
}
