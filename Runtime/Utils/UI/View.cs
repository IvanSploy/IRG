using System;

namespace IRG
{
    public abstract class View<TViewModel> : View where TViewModel : ViewModel
    {
        private static int _viewCount;
        protected TViewModel _viewModel;

        public void OnEnable()
        {
            _viewCount++;
            OnEnableUI();
            _viewModel ??= Activator.CreateInstance<TViewModel>();
            OnViewModelSet();
        }
        
        protected abstract void OnEnableUI();
        protected abstract void OnViewModelSet();

        protected override void OnBeforeDestroy()
        {
            _viewCount--;

            if (_viewCount == 0)
            {
                _viewModel.Dispose();
                _viewModel = null;
                OnAllViewsDestroyed();
            }
        }
        
        protected virtual void OnAllViewsDestroyed() {}
    }
    
    public abstract class View : Disposer
    {
        
    }
}