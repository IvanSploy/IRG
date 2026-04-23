using IRG.Interaction;
using UnityEngine;

namespace IRG
{
    public class PressurePlateBehaviour : Interactor
    {
        [SerializeField] private string _tag;
        [SerializeField] private Transform _mesh;

        private int _count;

        private void OnTriggerEnter(Collider other)
        {
            if (!IsValid(other)) return;
            if(_count == 0) Press(other.gameObject);
            _count++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsValid(other)) return;
            _count--;
            if (_count == 0) UnPress();
        }
        
        private bool IsValid(Collider other)
        {
            if (other.isTrigger) return false;
            if (!string.IsNullOrEmpty(_tag) && !other.CompareTag(_tag)) return false;
            return true;
        }

        private void Press(GameObject interactor)
        {
            var position = _mesh.position;
            position.y -= 0.045f;
            _mesh.position = position;

            var scale = _mesh.localScale;
            scale.y = 0.01f;
            _mesh.localScale = scale;
            
            Interact(interactor);
        }

        private void UnPress()
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
