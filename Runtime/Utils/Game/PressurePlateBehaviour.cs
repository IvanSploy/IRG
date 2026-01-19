using UnityEngine;
using UnityEngine.Events;

namespace IRG
{
    public class PressurePlateBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform _mesh;

        public UnityEvent OnPressed;

        private int _count;

        private bool _isPressed;

        public bool IsPressed
        {
            get => _isPressed;
            set
            {
                if (_isPressed == value) return;
                _isPressed = value;
                if (_isPressed)
                {
                    OnPressed?.Invoke();

                    var position = _mesh.position;
                    position.y -= 0.045f;
                    _mesh.position = position;

                    var scale = _mesh.localScale;
                    scale.y = 0.01f;
                    _mesh.localScale = scale;

                }
                else
                {
                    var position = _mesh.position;
                    position.y += 0.045f;
                    _mesh.position = position;

                    var scale = _mesh.localScale;
                    scale.y = 0.1f;
                    _mesh.localScale = scale;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger) return;
            IsPressed = true;
            _count++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger) return;
            _count--;
            if (_count == 0) IsPressed = false;
        }
    }
}
