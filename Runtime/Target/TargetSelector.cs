using System;
using UnityEngine;
using IRG.Interaction;

namespace IRG.Target
{
    public class TargetSelector : GameSystem
    {
        [SerializeField] private TargetDetector _detector;
        [SerializeField] private Transform _forwardTransform;
        [SerializeField] private LayerMask _excludeMask;
        
        public Interactable Selected { get; private set; }
        
        public event Action<Interactable> OnTargetSelected;
        public readonly ReactiveProperty<bool> IsPressed = new();

        private Camera _camera;
        
        private void Awake()
        {
            if(!_detector) _detector = GetComponentInChildren<TargetDetector>();
        }

        private void Start()
        {
            if(!_camera) _camera = Camera.main;
        }

        private void Update()
        {
            //TODO: Check when should disable
            if (!IsEnabled) return;
            SelectBestTarget();
        }

        public void StartTarget()
        {
            IsPressed.Value = (bool)Selected;
        }

        public void ConfirmTarget()
        {
            if (IsPressed.Value) Selected?.Interact();
            IsPressed.Value = false;
        }

        protected override void OnSystemDisabled()
        {
            if (Selected) SelectTarget(null);
        }

        private void SelectBestTarget()
        {
            var target = GetBestTarget(_camera.transform.position, _camera.transform.forward);
            if(!target) target = GetBestTarget(transform.position, _forwardTransform.forward);
            SelectTarget(target);
        }

        private Interactable GetBestTarget(Vector3 pos, Vector3 forward)
        {
            Interactable bestInteractionTarget = null;
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

        private void SelectTarget(Interactable interactable)
        {
            if(Selected == interactable) return;
            IsPressed.Value = false;
            Selected = interactable;
            OnTargetSelected?.Invoke(Selected);
        }
    }
}