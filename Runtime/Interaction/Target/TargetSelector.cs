using System;
using UnityEngine;

namespace IRG.Interaction.Target
{
    public class TargetSelector : MonoBehaviour
    {
        [SerializeField] private TargetDetector _detector;
        [SerializeField] private Transform _forwardTransform;
        [SerializeField] private LayerMask _excludeMask;

        private Target _selected;
        public Target Selected
        {
            get => _selected;
            private set
            {
                if(_selected == value) return;
                _selected = value;
                IsPressed.Value = false;
                OnTargetSelected?.Invoke(_selected);
            }
        }
        
        public event Action<Target> OnTargetSelected;
        public readonly ReactiveProperty<bool> IsPressed = new();

        private Camera _camera;
        
        private void Awake()
        {
            if(!_detector) _detector = GetComponentInChildren<TargetDetector>();
        }
        
        private void OnDisable()
        {
            Selected = null;
        }

        private void Start()
        {
            if(!_camera) _camera = Camera.main;
        }

        private void Update()
        {
            SelectBestTarget();
        }

        public void StartTarget()
        {
            IsPressed.Value = (bool)Selected;
        }

        public void ConfirmTarget()
        {
            if (IsPressed.Value) Selected?.Interact(gameObject);
            IsPressed.Value = false;
        }

        private void SelectBestTarget()
        {
            var target = GetBestTarget(_camera.transform.position, _camera.transform.forward);
            if(!target) target = GetBestTarget(transform.position, _forwardTransform.forward);
            Selected = target;
        }

        private Target GetBestTarget(Vector3 pos, Vector3 forward)
        {
            Target bestInteractionTarget = null;
            float max = float.MinValue;
            foreach (var target in _detector.Targets)
            {
                if(!target) continue;
                if(!target.IsInteractable) continue;

                var distance = target.transform.position - pos;
                
                var dir = distance.normalized;
                var dot = Vector3.Dot(dir, forward);
                if (max < dot)
                {
                    if (!Physics.Raycast(new Ray(pos, dir), out RaycastHit hit, 10, ~_excludeMask.value))
                        continue;

                    if(hit.collider.gameObject != target.gameObject) 
                        continue;
                    
                    max = dot;
                    bestInteractionTarget = target;
                }
            }

            return bestInteractionTarget;
        }
    }
}