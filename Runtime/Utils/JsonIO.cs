using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace IRG
{
    public static class JsonIO<TData>
    {
        private const string Folder = "Content";
        private static string GetFullPath(string path) => $"{Application.dataPath.Replace("/Assets", "")}/{Folder}/{path}.json";
        
        public static List<TData> Load(string path)
        {
            var fullPath = GetFullPath(path);
            if (!File.Exists(fullPath)) return null;
            
            var json = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<List<TData>>(json);
        }

        public static void Save(string path, List<TData> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var fullPath = GetFullPath(path);
            if (!File.Exists(fullPath))
            {
                var lastIndex = fullPath.LastIndexOf("/", StringComparison.InvariantCulture);
                var directory = fullPath.Substring(0, lastIndex);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            File.WriteAllText(fullPath, json);
        }
    }
}