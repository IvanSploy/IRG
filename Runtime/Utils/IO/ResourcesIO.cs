using System.IO;
using UnityEngine;

namespace IRG
{
    public static class ResourcesIO
    {
        public static string GetFullPath(string folder) => $"{Application.dataPath}/Resources/{folder}";
        public static string GetPath(string folder, string fileName) => $"Assets/Resources/{folder}/{fileName}.asset";
        public static string Combine(string s1, string s2) => $"{s1}/{s2}";

        public static TScriptableObject Load<TScriptableObject>(string folder, string fileName) where TScriptableObject : ScriptableObject
        {
            var so = Resources.Load<TScriptableObject>(Combine(folder, fileName));
            if (so == null)
            {
                so = Create<TScriptableObject>(folder, fileName);
            }

            return so;
        }
        
        public static TScriptableObject Create<TScriptableObject>(string folder, string fileName) where TScriptableObject : ScriptableObject
        {
            var directory = GetFullPath(folder);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            var so = Resources.Load<TScriptableObject>(Combine(folder, fileName));
            if (so == null)
            {
                so = ScriptableObject.CreateInstance<TScriptableObject>();
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.CreateAsset(so, GetPath(folder, fileName));
#endif
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(so);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
            return so;
        }

        public static void Save(Object obj)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(obj);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}