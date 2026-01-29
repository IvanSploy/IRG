using UnityEngine;

namespace IRG
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private bool _applyOffset;
        [SerializeField] private Vector3 _offset;

        private Camera _camera;
        
        private void OnValidate()
        {
            _offset.x = Mathf.Clamp(_offset.x, -1, 1);
            _offset.y = Mathf.Clamp(_offset.y, -1, 1);
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            transform.forward = _camera.transform.forward;

            if (!_applyOffset) return;
            
            var worldDisplacement = new Vector3(_offset.x, _offset.y, 0);
            var p = transform.position + _camera.transform.TransformVector(worldDisplacement);
            
            var screenPos = _camera.WorldToScreenPoint(p);
            var screenRay = _camera.ScreenPointToRay(screenPos);
            transform.position = p + screenRay.direction * _offset.z;
        }
    }
}