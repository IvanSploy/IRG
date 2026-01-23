using System;
using System.Collections.Generic;
using UnityEngine;

namespace IRG.Interaction.Target
{
    public class TargetDetector : MonoBehaviour
    {
        public readonly HashSet<Target> Targets = new();

        public event Action OnTargetsUpdated;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target)) return;
            Targets.Add(target);
            OnTargetsUpdated?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target)) return;
            Targets.Remove(target);
            OnTargetsUpdated?.Invoke();
        }
    }
}