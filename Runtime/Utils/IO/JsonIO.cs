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
        private static string GetFullPath(string fileName) => $"{Application.dataPath.Replace("/Assets", "")}/{Folder}/{fileName}.json";
        
        public static List<TData> Load(string fileName)
        {
            var path = GetFullPath(fileName);
            if (!File.Exists(path)) return null;
            
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<TData>>(json);
        }

        public static void Save(string fileName, List<TData> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var path = GetFullPath(fileName);
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
    }
}