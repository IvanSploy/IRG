using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IRG.Interaction
{
    public abstract class Interactor : MonoBehaviour
    {
        private readonly List<IInteractable> _interactableList = new();
        
        public bool IsInteractable => _interactableList.Any(interactable => interactable.IsInteractable);

        private void Awake()
        {
            _interactableList.AddRange(GetComponentsInChildren<IInteractable>());
        }
        
        public bool Interact()
        {
            bool interacted = false;
            foreach (var interactable in _interactableList)
            {
                if (!interactable.IsInteractable) continue;
                interactable.Interact();
                interacted = true;
            }
            return interacted;
        }
    }
}