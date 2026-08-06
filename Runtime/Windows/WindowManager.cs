using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IRG.Windows
{
    //TODO: Convertir a clase estatica sin Unity.
    public class WindowManager : MonoBehaviour
    {
        public static WindowManager Instance;

        public bool ShowOnlyCurrentLevel;
        public List<string> ShowAlwaysWindows = new();
        private readonly HashSet<string> _showAlwaysSet = new();
        
        private readonly Dictionary<string, IWindow> _allWindows = new();
        private readonly List<HashSet<string>> _currentWindows = new();

        public static event Action<string> OnWindowOpen;
        public static event Action<string> OnWindowClosed;

        public int CurrentLevel => _currentWindows.Count - 1;
        [NonSerialized] public bool Locked;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;

            RefreshShowAlwaysWindows();

            var windows = GetComponentsInChildren<IWindow>();
            foreach (var window in windows)
            {
                _allWindows.Add(window.Name, window);
                if (window.IsOpen) AddWindow(window.Level, window.Name);
            }

            Refresh();
        }

        private void OnValidate()
        {
            RefreshShowAlwaysWindows();
        }

        private void RefreshShowAlwaysWindows()
        {
            _showAlwaysSet.Clear();
            foreach (var windowName in ShowAlwaysWindows)
            {
                _showAlwaysSet.Add(windowName);
            }
        }

        public IReadOnlyCollection<string> Get(int level)
        {
            if (level >= _currentWindows.Count) return null;
            return _currentWindows[level];
        }

        public bool HasWindow(int level, string windowName)
        {
            if (level >= _currentWindows.Count) return false;
            return _currentWindows[level].Contains(windowName);
        }
        
        public bool HasWindow(string windowName)
        {
            foreach (var levelWindows in _currentWindows)
            {
                if (levelWindows.Contains(windowName)) 
                    return true;
            }
            return false;
        }

        public void Toggle(string windowName)
        {
            if(HasWindow(windowName)) Close(windowName);
            else Open(windowName);
        }

        public void Open(string windowName)
        {
            if (!_allWindows.TryGetValue(windowName, out var window)) return;
            if (!AddWindow(windowName)) return;
            window.Open();
            Refresh();
            OnWindowOpen?.Invoke(windowName);
        }
        
        public void Close(string windowName)
        {
            if (!_allWindows.TryGetValue(windowName, out var window)) return;
            if (!RemoveWindow(windowName)) return;
            window.Close();
            Refresh();
            OnWindowClosed?.Invoke(windowName);
        }
        
        public void CloseCurrentLevel()
        {
            if (CurrentLevel < 0) return;
            var windowNames = _currentWindows[CurrentLevel];
            foreach (var windowName in windowNames.ToArray())
            {
                Close(windowName);
            }
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
        
        private bool AddWindow(string windowName)
        {
            var window = _allWindows[windowName];
            return AddWindow(window.Level, windowName);
        }
        
        private bool AddWindow(int level, string windowName)
        {
            while (level >= _currentWindows.Count)
            {
                _currentWindows.Add(new HashSet<string>());
            }
            
            var set = _currentWindows[level];
            return set.Add(windowName);
        }
        
        private bool RemoveWindow(string windowName)
        {
            var window = _allWindows[windowName];
            return RemoveWindow(window.Level, windowName);
        }
        
        private bool RemoveWindow(int level, string windowName)
        {
            if(level >= _currentWindows.Count) return false;
            
            var removed = _currentWindows[level].Remove(windowName);
            if (!removed) return false;

            if (level != CurrentLevel) return true;
            
            while (CurrentLevel >= 0 && _currentWindows[CurrentLevel].Count == 0)
            {
                _currentWindows.RemoveAt(CurrentLevel);
            }

            return true;
        }

        public void Refresh()
        {
            if (CurrentLevel > 0) Lock();
            else UnLock();
            
            if (!ShowOnlyCurrentLevel) return;

            for (var level = 0; level < _currentWindows.Count; level++)
            {
                var levelWindows = _currentWindows[level];
                foreach (var windowName in levelWindows)
                {
                    if(_showAlwaysSet.Contains(windowName)) continue;
                    
                    var window = _allWindows[windowName];
                    if(level == CurrentLevel) window.Open();
                    else window.Close();
                }
            }
        }
    }
}
