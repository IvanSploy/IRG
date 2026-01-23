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

        private TargetSelector _targetSelector;
        private Color _currentColor;

        private bool _isVisible;

        private void Awake()
        {
            if (!_image) _image = GetComponentInChildren<Image>();
            
            var player = GameObject.FindWithTag("Player");
            _targetSelector = player.GetComponent<TargetSelector>();

            _targetSelector.OnTargetSelected += OnTargetSelected;
            _targetSelector.IsPressed.Subscribe(OnPressed);
            
            SetVisible(false);
        }

        private void OnDestroy()
        {
            if (_targetSelector)
            {
                _targetSelector.OnTargetSelected -= OnTargetSelected;
                _targetSelector.IsPressed.UnSubscribe(OnPressed);
            }
        }

        private void OnPressed(bool isPressed)
        {
            SetColor(isPressed ? _pressedColor : _defaultColor);
        }

        private void OnTargetSelected(Interactable target)
        {
            SetVisible((bool)target);
        }
        
        public void SetColor(Color color)
        {
            color.a = 1f;
            _currentColor = color;
            if (_isVisible)
            {
                _image.color = color;
            }
        }

        private void Update()
        {
            if(_targetSelector.Selected) transform.position = _targetSelector.Selected.transform.TransformPoint(_offset);
        }

        private void SetVisible(bool visible)
        {
            _isVisible = visible;
            Color color = _currentColor;
            if (!_isVisible)
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