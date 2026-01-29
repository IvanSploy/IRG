using IRG.Interaction;
using UnityEngine;

namespace IRG
{
    public class Trigger : Interactor
    {
        [SerializeField] protected string _tag;
        private int _count;

        private bool _active;

        private void OnTriggerEnter(Collider other)
        {
            if(!IsValid(other)) return;
            _count++;
            if (_active) return;
            Interact();
            _active = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if(!IsValid(other)) return;
            _count--;
            if (_count == 0) _active = false;
        }

        private bool IsValid(Collider other)
        {
            if (other.isTrigger) return false;
            if (!string.IsNullOrEmpty(_tag) && !other.CompareTag(_tag)) return false;
            return true;
        }
    }
}
