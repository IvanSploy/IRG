using System;
using System.Collections.Generic;
using UnityEngine;

namespace IRG
{
    public class Disposer : MonoBehaviour
    {
        protected readonly List<IDisposable> _disposables = new();
        protected virtual void OnBeforeDestroy(){}
        private void OnDestroy()
        {
            OnBeforeDestroy();
            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
            _disposables.Clear();
            OnAfterDestroy();
        }
        protected virtual void OnAfterDestroy(){}
    }
}