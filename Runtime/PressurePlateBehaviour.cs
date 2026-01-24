using IRG.Interaction;
using UnityEngine;

namespace IRG
{
    public class PressurePlateBehaviour : Interactor
    {
        [SerializeField] private Transform _mesh;

        private int _count;

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger) return;
            if(_count == 0) Press();
            _count++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger) return;
            _count--;
            if (_count == 0) UnPress();
        }

        public void Press()
        {
            var position = _mesh.position;
            position.y -= 0.045f;
            _mesh.position = position;

            var scale = _mesh.localScale;
            scale.y = 0.01f;
            _mesh.localScale = scale;
            
            Interact();
        }

        public void UnPress()
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
