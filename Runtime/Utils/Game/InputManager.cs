using UnityEngine.InputSystem;

namespace IRG
{
    public class InputManager : GameSystem
    {
        public static InputManager Instance;

        public InputActionAsset InputActionsAsset;

        public static InputActionMap PlayerMap;
        public static InputActionMap UIMap;
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            PlayerMap = InputActionsAsset.FindActionMap("Player");
            UIMap = InputActionsAsset.FindActionMap("UI");
        }
        
        protected override void OnSystemEnabled()
        {
            CursorManager.Lock();
            PlayerMap.Enable();
        }

        protected override void OnSystemDisabled()
        {
            CursorManager.Release();
            PlayerMap.Disable();
        }

        public void OnEnable()
        {
            InputActionsAsset.Enable();
        }

        private void OnDisable()
        {
            InputActionsAsset.Disable();
        }
    }
}