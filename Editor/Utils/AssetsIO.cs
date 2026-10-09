using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace IRG.Editor
{
    public static class AssetsIO
    {
        public static string GetDirectoryPath(string folder) => $"{Application.dataPath}/{folder}";
        public static string GetFilePath(string path) => $"{Application.dataPath}/{path}.asset";
        public static string GetAssetPath(string path) => $"Assets/{path}.asset";
        public static string Combine(string folder, string fileName) => $"{folder}/{fileName}".Replace("//", "/");

        public static T Load<T>(string path) where T : ScriptableObject
        {
            return AssetDatabase.LoadAssetAtPath<T>(GetAssetPath(path));
        }
        
        public static Object Load(string path, Type type)
        {
            return AssetDatabase.LoadAssetAtPath(GetAssetPath(path), type);
        }
        
        public static IEnumerable<T> LoadAll<T>(string folder = "") where T : ScriptableObject
        {
            var list = new List<T>();
            var allPaths = FindAll<T>(folder);
            foreach (var path in allPaths)
            {
                list.Add(Load<T>(path));
            }
            return list;
        }
        
        public static IEnumerable<Object> LoadAll(Type type, string folder = "")
        {
            var list = new List<Object>();
            var allPaths = FindAll(type, folder);
            foreach (var path in allPaths)
            {
                list.Add(Load(path, type));
            }
            return list;
        }
        
        public static IEnumerable<string> FindAll<T>(string folder = "") where T : ScriptableObject
        {
            return FindAll(typeof(T), folder);
        }
        
        public static IEnumerable<string> FindAll(Type type, string folder = "")
        {
            List<string> list = new List<string>();
            string filter = $"t:{type.Name}";
            var guids = AssetDatabase.FindAssets(filter, new[] {$"Assets/{folder}"});
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                list.Add(path.Replace("Assets/", "").Replace(".asset", ""));
            }
            return list;
        }

        public static bool Exists(string path)
        {
            return AssetDatabase.AssetPathExists(GetAssetPath(path));
        }
        
        public static T Create<T>(string path) where T : ScriptableObject
        {
            string folderPath = path.GetFolder();
            string elementName = path.GetFileName();
            if(string.IsNullOrEmpty(elementName)) elementName = $"New {typeof(T).Name}";
            string fileName = elementName;
            string finalPath;
            
            int index = 0;
            while(Exists(finalPath = Combine(folderPath, fileName)))
            {
                fileName = $"{elementName} {index}";
                index++;
            }

            CreateFolder(folderPath);
            
            var so = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(so, GetAssetPath(finalPath));
            Save(so);
            return so;
        }
        
        public static void RenameAsset(string path, string newName)
        {
            AssetDatabase.RenameAsset(GetAssetPath(path), newName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public static void Remove(string path)
        {
            AssetDatabase.DeleteAsset(GetAssetPath(path));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public static void Save(Object obj)
        {
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public static void CreateFolder(string folder)
        {
            var directory = GetDirectoryPath(folder);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        
        public static void RemoveFolder(string folder, bool force = false)
        {
            var directory = GetDirectoryPath(folder);
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, force);
            }
        }
    }
}