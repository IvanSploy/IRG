using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IRG.Interaction
{
    public class Interactable : MonoBehaviour
    {
        private readonly List<IInteractable> _interactableList = new();

        private void Awake()
        {
            _interactableList.AddRange(GetComponentsInChildren<IInteractable>());
        }

        public bool IsInteractable => _interactableList.Any(interactable => interactable.IsInteractable);
        
        public void Interact()
        {
            foreach (var interactable in _interactableList)
            {
                if(interactable.IsInteractable) interactable.Interact();
            }
        }
    }
}