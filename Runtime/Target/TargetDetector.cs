using System;
using System.Collections.Generic;
using UnityEngine;
using IRG.Interaction;

namespace IRG.Target
{
    public class TargetDetector : MonoBehaviour
    {
        public readonly List<Interactable> Targets = new();

        public event Action OnTargetsUpdated;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Interactable>(out var target)) return;
            Targets.Add(target);
            OnTargetsUpdated?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Interactable>(out var target)) return;
            Targets.Remove(target);
            OnTargetsUpdated?.Invoke();
        }
    }
}