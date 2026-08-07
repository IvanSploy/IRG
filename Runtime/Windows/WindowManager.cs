using System;
using System.Collections.Generic;
using System.Linq;

namespace IRG.Windows
{
    public static class WindowManager
    {
        public static bool ShowOnlyCurrentLevel;
        
        private static readonly Dictionary<string, Window> _allWindows = new();
        private static readonly List<HashSet<string>> _currentWindows = new();
        private static readonly HashSet<string> _showAlwaysSet = new();

        public static event Action<string> OnWindowOpen;
        public static event Action<string> OnWindowClosed;

        public static int CurrentLevel => _currentWindows.Count - 1;
        [NonSerialized] public static bool Locked;

        public static void RegisterWindow(Window window)
        {
            _allWindows.Add(window.Key, window);
            if(window.ShowAlwaysWhenOpen) _showAlwaysSet.Add(window.Key);
            if (window.IsOpen) AddWindow(window.Level, window.Key);
            Refresh();
        }

        public static void UnRegisterWindow(Window window)
        {
            if(window.ShowAlwaysWhenOpen) _showAlwaysSet.Remove(window.Key);
            RemoveWindow(window.Level, window.Key);
            _allWindows.Remove(window.Key);
            Refresh();
        }

        public static IReadOnlyCollection<string> Get(int level)
        {
            if (level >= _currentWindows.Count) return null;
            return _currentWindows[level];
        }

        public static bool HasWindow(int level, string windowName)
        {
            if (level >= _currentWindows.Count) return false;
            return _currentWindows[level].Contains(windowName);
        }
        
        public static bool HasWindow(string windowName)
        {
            foreach (var levelWindows in _currentWindows)
            {
                if (levelWindows.Contains(windowName)) 
                    return true;
            }
            return false;
        }

        public static void Toggle(string windowName)
        {
            if(HasWindow(windowName)) Close(windowName);
            else Open(windowName);
        }

        public static void Open(string windowName)
        {
            if (!_allWindows.TryGetValue(windowName, out var window)) return;
            if (!AddWindow(windowName)) return;
            window.Open();
            Refresh();
            OnWindowOpen?.Invoke(windowName);
        }
        
        public static void Close(string windowName)
        {
            if (!_allWindows.TryGetValue(windowName, out var window)) return;
            if (!RemoveWindow(windowName)) return;
            window.Close();
            Refresh();
            OnWindowClosed?.Invoke(windowName);
        }
        
        public static void CloseCurrentLevel()
        {
            if (CurrentLevel < 0) return;
            var windowNames = _currentWindows[CurrentLevel];
            foreach (var windowName in windowNames.ToArray())
            {
                Close(windowName);
            }
        }
        
        public static void Lock()
        {
            Locked = true;
            if (Locked) CursorManager.Lock("Windows");
            else CursorManager.Unlock("Windows");
        }
        
        public static void UnLock()
        {
            Locked = false;
            if (Locked) CursorManager.Lock("Windows");
            else CursorManager.Unlock("Windows", true);
        }
        
        private static bool AddWindow(string windowName)
        {
            var window = _allWindows[windowName];
            return AddWindow(window.Level, windowName);
        }
        
        private static bool AddWindow(int level, string windowName)
        {
            while (level >= _currentWindows.Count)
            {
                _currentWindows.Add(new HashSet<string>());
            }
            
            var set = _currentWindows[level];
            return set.Add(windowName);
        }
        
        private static bool RemoveWindow(string windowName)
        {
            var window = _allWindows[windowName];
            return RemoveWindow(window.Level, windowName);
        }
        
        private static bool RemoveWindow(int level, string windowName)
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

        public static void Refresh()
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
