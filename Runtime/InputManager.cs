using UnityEngine;
using UnityEngine.InputSystem;

namespace IRG
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        public InputActionAsset InputActionsAsset;
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (InputActionsAsset == null) InputActionsAsset = InputSystem.actions;
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