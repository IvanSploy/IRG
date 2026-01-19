using System;
using UnityEngine;

namespace IRG
{
    public class GameSystem : MonoBehaviour
    {
        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value) return;
                _isEnabled = value;
                if (_isEnabled) OnSystemEnabled();
                else OnSystemDisabled();
                OnEnabledChanged?.Invoke(_isEnabled);
            }
        }
        
        protected virtual void OnSystemEnabled() {}
        protected virtual void OnSystemDisabled() {}

        public event Action<bool> OnEnabledChanged;
        public void ClearEvents() => OnEnabledChanged = null;
    }
}