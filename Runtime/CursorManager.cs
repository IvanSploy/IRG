using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IRG
{
    public class CursorManager : MonoBehaviour
    {
        public static CursorManager Instance;
        
        [SerializeField] private bool _startLocked;

        private static readonly HashSet<string> Locks = new();
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            if (_startLocked) LockMouse();
        }

        private void OnDestroy()
        {
            ReleaseMouse();
        }

        public static void Lock(string name)
        {
            if (Locks.Add(name) && Locks.Count == 1) ReleaseMouse();
        }

        public static void Unlock(string name)
        {
            Locks.Remove(name);
            if(Locks.Count == 0) LockMouse();
        }

        private static void LockMouse()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2f, Screen.height / 2f));
        }

        private static void ReleaseMouse()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}