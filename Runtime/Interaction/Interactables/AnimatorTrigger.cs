using UnityEngine;

namespace IRG.Interaction
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorTrigger : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _triggerName = "Trigger";
        private Animator _animator;

        public bool IsInteractable => true;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        public void Interact(GameObject interactor)
        {
            _animator.SetTrigger(_triggerName);
        }
    }
}