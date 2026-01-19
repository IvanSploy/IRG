using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IRG
{
    public static class ResourcesDataAccess<TData> 
        where TData : Object
    {
        public static readonly List<TData> Data = new();
        
        public static void Load()
        {
            var list = Resources.LoadAll<TData>("");
            SetAll(list);
        }
        
        public static List<TData> GetAll()
        {
            if (Data.Count == 0) Load();
            return Data.ToList();
        }
        
        public static void SetAll(IEnumerable<TData> e)
        {
            Data.Clear();
            foreach (var data in e)
            {
                Data.Add(data);
            }
        }
    }
}