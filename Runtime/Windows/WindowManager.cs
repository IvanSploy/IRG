using System;
using System.Collections.Generic;
using UnityEngine;

namespace IRG.Windows
{
    public class WindowManager : MonoBehaviour
    {
        public static WindowManager Instance;
        
        private readonly Dictionary<string, List<IWindow>> _windows = new();
        private readonly HashSet<string> _openedWindows = new();

        public event Action OnWindowsUpdated;

        private static IWindow _example;
        
        public static bool AnyWindowOpened => Instance?._openedWindows.Count > 0;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;

            var windows = GetComponentsInChildren<IWindow>();
            foreach (var window in windows)
            {
                _windows.AddToList(window.Name, window);
                if (window.IsOpen) _openedWindows.Add(window.Name);
            }

            CheckCursor();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
            {
                var windowName = "Inventory";
                var isOpen = _openedWindows.Contains(windowName);
                if(isOpen) Close(windowName);
                else Open(windowName);
            }
        }

        public void Open(string windowName)
        {
            if (!_windows.TryGetValue(windowName, out var windows)) return;
            if (!_openedWindows.Add(windowName)) return;
            foreach (var window in windows)
            {
                window.Open();
            }

            RefreshWindows();
        }
        
        public void Close(string windowName)
        {
            if (!_windows.TryGetValue(windowName, out var windows)) return;
            if (!_openedWindows.Remove(windowName)) return;
            foreach (var window in windows)
            {
                window.Close();
            }

            RefreshWindows();
        }

        public void RefreshWindows()
        {
            CheckCursor();
            OnWindowsUpdated?.Invoke();
        }
        
        private void CheckCursor()
        {
            if (_openedWindows.Count == 0) CursorManager.Lock();
            else CursorManager.Release();
        }
    }
}
