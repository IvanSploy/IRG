using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace IRG.Editor
{
    public static class AssetUtils
    {
        public static T CreateAsset<T>(string path, string assetName, Action<T> onCreated = null) where T : ScriptableObject
        {
            if(!path.EndsWith("/")) path += "/";
            
            T asset = ScriptableObject.CreateInstance<T>();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            int index = 0;
            string finalPath;
            do
            {
                finalPath = path + $"{assetName} {index}.asset";
                index++;
            } while (File.Exists(finalPath));
            
            onCreated?.Invoke(asset);
            
            AssetDatabase.CreateAsset(asset, finalPath);
            AssetDatabase.SaveAssets();
        
            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
            
            return asset;
        }
        
        public static T CreateAssetAndStartRename<T>(string path, string assetName, Action<T> onCreated = null) where T : ScriptableObject
        {
            T asset = CreateAsset(path, assetName, onCreated);
            StartRenameForSelectedAsset();
            return asset;
        }
        
        public static void StartRenameForSelectedAsset()
        {
            EditorApplication.delayCall += () =>
            {
                var e = new Event
                {
                    keyCode = KeyCode.F2,
                    type = EventType.KeyDown,
                };
                EditorWindow.focusedWindow?.SendEvent(e);
            };
        }
    }
}