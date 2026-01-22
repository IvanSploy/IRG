using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace IRG.Target
{
    public class InputTarget : MonoBehaviour
    {
        [FormerlySerializedAs("_clickAction")] 
        [SerializeField] private InputActionReference _action;
        [SerializeField] private TargetSelector _targetSelector;
        
        public readonly ReactiveProperty<bool> IsPressed = new();

        private void OnEnable()
        {
            _action.action.started += OnClickPressed;
            _action.action.canceled += OnClickReleased;
        }

        private void OnDisable()
        {
            _action.action.started -= OnClickPressed;
            _action.action.canceled -= OnClickReleased;
        }

        private void OnClickPressed(InputAction.CallbackContext context)
        {
            IsPressed.Value = (bool)_targetSelector.Selected;
        }

        private void OnClickReleased(InputAction.CallbackContext context)
        {
            if (IsPressed.Value) _targetSelector.Selected?.Interact();
            IsPressed.Value = false;
        }
    }
}