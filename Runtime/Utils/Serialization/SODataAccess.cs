using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace IRG
{
    public abstract class SODataAccess<TScriptableObject, TData> where TScriptableObject : ScriptableObjectList<TData>
    {
        public virtual string FileName => "Default";
        public const string Folder = "ScriptableObjects";
        public static readonly string LongPath = $"{Application.dataPath}/Resources/{Folder}";
        public static readonly string Path = $"Assets/Resources/{Folder}";
        public static string GetPath(string fileName) => $"{Folder}/{fileName}";
        public static string GetFullPath(string fileName) => $"{Path}/{fileName}.asset";

        private TScriptableObject _so;
        
        public void Load()
        {
            var path = GetPath(FileName);
            _so = Resources.Load<TScriptableObject>(path);
            if (_so == null)
            {
                Save();
            }
        }

        public void Save()
        {
            if (!Directory.Exists(LongPath))
            {
                Directory.CreateDirectory(LongPath);
            }
            
            var path = GetPath(FileName);
            _so = Resources.Load<TScriptableObject>(path);
            if (_so == null)
            {
                _so = ScriptableObject.CreateInstance<TScriptableObject>();
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.CreateAsset(_so, GetFullPath(FileName));
#endif
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(_so);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
        
        public int Count => _so.Data.Count;
        
        public void Clear()
        {
            _so.Data.Clear();
        }
        
        public TData Get(int index)
        {
            return _so.Data[index];
        }
        
        public List<TData> GetAll()
        {
            return _so.Data.ToList();
        }

        public TData Find(Predicate<TData> predicate)
        {
            return _so.Data.Find(predicate);
        }
        
        public int FindIndex(Predicate<TData> predicate)
        {
            return _so.Data.FindIndex(predicate);
        }
        
        public void Add(TData data)
        {
            _so.Data.Add(data);
        }
        
        public void Set(int index, TData data)
        {
            _so.Data[index] = data;
        }

        public void Remove(int index)
        {
            _so.Data.RemoveAt(index);
        }
    }
}