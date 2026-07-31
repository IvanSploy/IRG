namespace IRG
{
    public abstract class View<TViewModel> : View where TViewModel : ViewModel, new()
    {
        private int _viewId;
        private static int _viewCount;
        protected static TViewModel _viewModel;

        public void OnEnable()
        {
            _viewId = _viewCount;
            _viewCount++;
            OnEnableUI();
            _viewModel ??= new TViewModel();
            OnViewModelSet();
        }
        
        protected abstract void OnEnableUI();
        protected abstract void OnViewModelSet();

        public void Update()
        {
            if (_viewId == 0) _viewModel?.OnUpdate();
            OnUpdate();
        }
        protected virtual void OnUpdate() { }

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
    
    public abstract class View : Disposer { }
}