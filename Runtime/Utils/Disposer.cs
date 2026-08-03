using System;
using System.Collections.Generic;
using UnityEngine;

namespace IRG
{
    public class Disposer : MonoBehaviour
    {
        protected readonly List<IDisposable> _disposables = new();
        private void OnDisable()
        {
            OnDisabled();
            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
            _disposables.Clear();
        }
        protected virtual void OnDisabled() { }
    }
}