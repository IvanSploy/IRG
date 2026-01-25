using UnityEditor;
using UnityEngine;
using System.IO;

namespace IRG.Editor
{
    public static class AssetsRefactor
    {
        public struct AssetInfo
        {
            public string Name;
            public string NameSpace;
            public string Assembly;
            
            public int GetCombabilityLevel()
            {
                int level = 0;
                if (!string.IsNullOrEmpty(Name))
                {
                    level = 1;
                    if (!string.IsNullOrEmpty(NameSpace))
                    {
                        level = 2;
                        if (!string.IsNullOrEmpty(Assembly))
                        {
                            level = 3;
                        }
                    }
                }
                
                return level;
            }
            
            public string GetValue()
            {
                string result = $"class: {Name}";
                if (!string.IsNullOrEmpty(NameSpace))
                {
                    result += $", ns: {NameSpace}";
                    if (!string.IsNullOrEmpty(Assembly))
                    {
                        result += $", asm: {Assembly}";
                    }
                }
                return result;
            }
        }
        public static void ReplaceInAssets(AssetInfo oldInfo, AssetInfo newInfo)
        {
            var oldLevel = oldInfo.GetCombabilityLevel();
            var newLevel = newInfo.GetCombabilityLevel();

            if (oldLevel == 0 || newLevel == 0 || oldLevel != newLevel)
            {
                Debug.LogError($"[Refactor] Refactor failed: with old level {oldLevel} and new level {newLevel}");
                return;
            }

            var oldText = oldInfo.GetValue();
            var newText = newInfo.GetValue();

            bool changed = false;
            foreach (var path in AssetDatabase.GetAllAssetPaths())
            {
                if (path.EndsWith(".prefab") || path.EndsWith(".unity") || path.EndsWith(".asset"))
                {
                    string text = File.ReadAllText(path);
                    if (text.Contains(oldText))
                    {
                        text = text.Replace(oldText, newText);
                        File.WriteAllText(path, text);
                        Debug.Log($"[Refactor] Updated {path}");
                        changed = true;
                    }
                }
            }
            if(changed) AssetDatabase.Refresh();
            else Debug.LogWarning("[Refactor] Refactor failed: no matches found");
        }
    }
}