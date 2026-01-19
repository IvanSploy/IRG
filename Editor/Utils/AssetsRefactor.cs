using UnityEditor;
using UnityEngine;
using System.IO;

namespace IRG.Editor
{
    public static class AssetsRefactor
    {
        public static void ReplaceInAssets(string oldName, string newName)
        {
            oldName = $"class: {oldName}";
            newName = $"class: {newName}";
            
            foreach (var path in AssetDatabase.GetAllAssetPaths())
            {
                if (path.EndsWith(".prefab") || path.EndsWith(".unity") || path.EndsWith(".asset"))
                {
                    string text = File.ReadAllText(path);
                    if (text.Contains(oldName))
                    {
                        text = text.Replace(oldName, newName);
                        File.WriteAllText(path, text);
                        Debug.Log($"[Refactor] Updated {path}");
                    }
                }
            }
            AssetDatabase.Refresh();
        }
    }
}