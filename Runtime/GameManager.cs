using System;
using System.Collections.Generic;
using UnityEngine;

namespace IRG
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        [Serializable]
        public struct GameSystemLock
        {
            public string LockType;
            public MonoBehaviour System;
        }
        
        [SerializeField] List<GameSystemLock> _systems;
        private readonly HashSet<string> _currentLocks = new();

        public void Lock(string lockType)
        {
            _currentLocks.Add(lockType);
            UpdateSystems();
        }

        public void UnLock(string lockType)
        {
            _currentLocks.Remove(lockType);
            UpdateSystems();
        }

        public void UpdateSystems()
        {
            foreach (GameSystemLock systemLock in _systems)
            {
                if(!systemLock.System) continue;
                systemLock.System.enabled = !_currentLocks.Contains(systemLock.LockType);
            }
        }
    }
}