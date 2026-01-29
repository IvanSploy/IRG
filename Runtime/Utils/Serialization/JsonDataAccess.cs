using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace IRG
{
    public static class JsonDataAccess<TData>
    {
        private const string Folder = "Content";
        private static string GetFullPath(string fileName) => $"{Application.dataPath}/{Folder}/{fileName}.json";
        
        public static readonly List<TData> Data = new();
        
        public static void Load(string fileName)
        {
            var path = GetFullPath(fileName);
            if (!File.Exists(path))
            {
                Save(fileName);
                return;
            }
            
            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<TData>>(json);
            list ??= new List<TData>();
            SetAll(list);
        }

        public static void Save(string fileName)
        {
            var json = JsonConvert.SerializeObject(Data, Formatting.Indented);
            var path =  GetFullPath(fileName);
            if (!File.Exists(path))
            {
                var lastIndex = path.LastIndexOf("/", StringComparison.InvariantCulture);
                var directory = path.Substring(0, lastIndex);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            File.WriteAllText(path, json);
        }
        
        public static int Count => Data.Count;
        
        public static void Clear()
        {
            Data.Clear();
        }
        
        public static TData Get(int index)
        {
            return Get(index);
        }
        
        public static List<TData> GetAll()
        {
            return Data.ToList();
        }

        public static TData Find(Predicate<TData> predicate)
        {
            return Data.Find(predicate);
        }
        
        public static int FindIndex(Predicate<TData> predicate)
        {
            return Data.FindIndex(predicate);
        }
        
        public static void Add(TData data)
        {
            Data.Add(data);
        }
        
        public static void Set(int index, TData data)
        {
            Data[index] = data;
        }
        
        public static void SetAll(IEnumerable<TData> e)
        {
            Data.Clear();
            foreach (var data in e)
            {
                Data.Add(data);
            }
        }

        public static void Remove(int index)
        {
            Data.RemoveAt(index);
        }
    }
}