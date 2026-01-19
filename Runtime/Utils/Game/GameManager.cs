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
        
        [Flags]
        public enum GameLockType
        {
            None = 0,
            Cinematic = 1,
            Dialogue = 2,
        }
        
        [Serializable]
        public struct GameSystemLock
        {
            public GameLockType LockType;
            public GameSystem System;
        }
        
        [SerializeField] List<GameSystemLock> _lockingSystems;
        [SerializeField] List<GameSystemLock> _lockedSystems;
        [SerializeField] private GameLockType _currentLockType;

        private readonly List<Action<bool>> _enabledChangedCallbacks = new();
        
        public GameLockType CurrentLockType
        {
            get => _currentLockType;
            private set
            {
                if (_currentLockType == value) return;
                _currentLockType = value;
                UpdateSystems();
            }
        }
        
        private void Start()
        {
            RegisterSystems();
        }

        private void OnDestroy()
        {
            UnRegisterSystems();
        }

        public void RegisterSystems()
        {
            UnRegisterSystems();
            for (var i = 0; i < _lockingSystems.Count; i++)
            {
                var systemLock = _lockingSystems[i];
                if (!systemLock.System) continue;

                Action<bool> callback = value =>
                {
                    if (value)
                    {
                        Lock(systemLock.LockType);
                    }
                    else
                    {
                        UnLock(systemLock.LockType);
                    }
                };
                _enabledChangedCallbacks.Add(callback);

                systemLock.System.OnEnabledChanged += callback;
                if (systemLock.System.IsEnabled) Lock(systemLock.LockType);
            }

            UpdateSystems();
        }

        public void UnRegisterSystems()
        {
            if (_enabledChangedCallbacks.Count == 0) return;
            
            for (var i = 0; i < _lockingSystems.Count; i++)
            {
                var systemLock = _lockingSystems[i];
                if (!systemLock.System) continue;
                
                systemLock.System.OnEnabledChanged -= _enabledChangedCallbacks[i];
            }
            
            _enabledChangedCallbacks.Clear();
            _currentLockType = GameLockType.None;
        }

        private void Lock(GameLockType lockType)
        {
            CurrentLockType |= lockType;
        }

        private void UnLock(GameLockType lockType)
        {
            CurrentLockType &= ~lockType;
        }

        public void UpdateSystems()
        {
            foreach (GameSystemLock systemLock in _lockedSystems)
            {
                if(!systemLock.System) continue;
                systemLock.System.IsEnabled = !CurrentLockType.HasAnyFlag(systemLock.LockType);
            }
        }
    }
}