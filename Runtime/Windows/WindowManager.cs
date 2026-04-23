using System;
using System.Collections.Generic;
using UnityEngine;

namespace IRG.Windows
{
    public class WindowManager : MonoBehaviour
    {
        public static WindowManager Instance;
        
        private readonly Dictionary<string, List<IWindow>> _windows = new();
        private readonly HashSet<string> _currentWindows = new();
        public HashSet<string> CurrentWindows => _currentWindows;

        public event Action<string> OnWindowOpen;
        public event Action<string> OnWindowClosed;

        public bool Locked;

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
                if (window.IsOpen) _currentWindows.Add(window.Name);
            }
        }

        public void Toggle(string windowName)
        {
            var isOpen = _currentWindows.Contains(windowName);
            if(isOpen) Close(windowName);
            else Open(windowName);
        }

        public void Open(string windowName)
        {
            if (!_windows.TryGetValue(windowName, out var windows)) return;
            if (!_currentWindows.Add(windowName)) return;
            foreach (var window in windows)
            {
                window.Open();
            }
            OnWindowOpen?.Invoke(windowName);
        }
        
        public void Close(string windowName)
        {
            if (!_windows.TryGetValue(windowName, out var windows)) return;
            if (!_currentWindows.Remove(windowName)) return;
            foreach (var window in windows)
            {
                window.Close();
            }
            OnWindowClosed?.Invoke(windowName);
        }
        
        public void Lock()
        {
            Locked = true;
            if (Locked) CursorManager.Lock("Windows");
            else CursorManager.Unlock("Windows");
        }
        
        public void UnLock()
        {
            Locked = false;
            if (Locked) CursorManager.Lock("Windows");
            else CursorManager.Unlock("Windows", true);
        }
    }
}
