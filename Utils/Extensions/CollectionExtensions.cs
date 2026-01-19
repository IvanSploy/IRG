using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

namespace IRG.Utils
{
    public static class CollectionExtensions
    {
        public static void AddItem<TA, TB>(this Dictionary<TA, List<TB>> dictionary, TA key, TB value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(value);
            }
            else
            {
                dictionary[key] = new List<TB>{ value };
            }
        }
        
        public static void RemoveItem<TA, TB>(this Dictionary<TA, List<TB>> dictionary, TA key, TB value)
        {
            if (!dictionary.ContainsKey(key)) return;
            
            dictionary[key].Remove(value);
            if (dictionary[key].Count == 0)
            {
                dictionary.Remove(key);
            }
        }
    }
}