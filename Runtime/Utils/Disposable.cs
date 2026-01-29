using System;
using System.Collections.Generic;

namespace IRG
{
    public class Disposable : IDisposable
    {
        protected readonly List<IDisposable> _disposables = new();

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
            _disposables.Clear();
        }
    }
}