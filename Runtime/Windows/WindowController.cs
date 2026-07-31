using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IRG.Windows
{
    public class WindowController : MonoBehaviour
    {
        [SerializeField] private InputActionReference _actionReference;
        [SerializeField] private List<string> _openOnFree = new();
        [SerializeField] private int _freeLevel;
        
        private void Awake()
        {
            _actionReference.action.performed += ctx =>
            {
                if (WindowManager.Instance.CurrentLevel > _freeLevel)
                {
                    WindowManager.Instance.CloseCurrentLevel();
                }
                else
                {
                    foreach (var window in _openOnFree)
                    {
                        WindowManager.Instance.Open(window);
                    }
                }
            };
        }
    }
}