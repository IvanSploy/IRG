using UnityEngine;
using UnityEngine.UI;
using IRG.Interaction;

namespace IRG.Target
{
    public class TargetRenderer : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Image _image;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _pressedColor;

        private InputTarget _inputTarget;
        private TargetSelector _targetSelector;
        private Color _currentColor;

        private Interactable _target;

        private void Awake()
        {
            if (!_image) _image = GetComponentInChildren<Image>();
            
            var player = GameObject.FindWithTag("Player");
            _inputTarget = player.GetComponent<InputTarget>();
            _targetSelector = player.GetComponent<TargetSelector>();

            if (_inputTarget) _inputTarget.IsPressed.Subscribe(OnPressed);
            if (_targetSelector) _targetSelector.OnTargetSelected += OnTargetSelected;
            
            SetVisible(false);
        }

        private void OnDestroy()
        {
            if (_inputTarget) _inputTarget.IsPressed.UnSubscribe(OnPressed);
            if (_targetSelector) _targetSelector.OnTargetSelected -= OnTargetSelected;
        }

        private void OnPressed(bool isPressed)
        {
            _currentColor = isPressed ? _pressedColor : _defaultColor;
        }

        private void OnTargetSelected(Interactable target)
        {
            _target = target;
            SetVisible((bool)_target);
        }

        private void Update()
        {
            if(_targetSelector.Selected) transform.position = _targetSelector.Selected.transform.TransformPoint(_offset);
        }

        private void SetVisible(bool visible)
        {
            Color color = _currentColor;
            if (!visible)
            {
                color.a = 0f;
                _image.color = color;
                return;
            }
            
            color.a = 1f;
            _image.color = color;
        }
    }
}