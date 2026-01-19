using UnityEngine;

namespace IRG
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private bool _startLocked;

        public static CursorManager Instance;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            if (_startLocked) Lock();
        }

        private void OnDestroy()
        {
            Release();
        }

        public static void Lock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public static void Release()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}