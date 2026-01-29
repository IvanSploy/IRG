using UnityEngine;
using UnityEngine.Events;

namespace IRG.Interaction
{
    public class InteractionEvent : MonoBehaviour, IInteractable
    {
        [field:SerializeField] public bool IsInteractable { get; set; } = true;
        public UnityEvent OnInteracted;
        
        public void Interact()
        {
            OnInteracted?.Invoke();
        }
    }
}