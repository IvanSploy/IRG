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
        
        public void Interact()
        {
            foreach (var interactable in _interactableList)
            {
                if(interactable.IsInteractable) interactable.Interact();
            }
        }
    }
}