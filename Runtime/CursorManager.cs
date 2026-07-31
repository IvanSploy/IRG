using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IRG
{
    public class CursorManager : MonoBehaviour
    {
        public static CursorManager Instance;
        
        [SerializeField] private bool _startLocked = true;

        private static readonly HashSet<string> Locks = new();
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            if (_startLocked) HideMouse();
            else Lock("CursorManager");
        }

        private void OnDestroy()
        {
            ShowMouse();
        }

        public static void Lock(string name)
        {
            if (Locks.Add(name) && Locks.Count == 1) ShowMouse();
        }

        public static void Unlock(string name, bool centerMouse = false)
        {
            Locks.Remove(name);
            if(Locks.Count == 0) HideMouse(centerMouse);
        }

        private static void HideMouse(bool centerMouse = false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if(centerMouse) Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2f, Screen.height / 2f));
        }

        private static void ShowMouse()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}