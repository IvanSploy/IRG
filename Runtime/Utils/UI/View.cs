using UnityEngine.UIElements;

namespace IRG
{
    public abstract class UIView<TViewModel> : View<TViewModel> where TViewModel : ViewModel, new()
    {
        private UIDocument _document;
        private PanelRenderer _panelRenderer;

        protected VisualElement _root { get; private set; }

        private new void Awake()
        {
            base.Awake();
            _panelRenderer = GetComponent<PanelRenderer>();
            _document = GetComponent<UIDocument>();
            OnAwake();
        }

        protected new void OnEnable()
        {
            OnEnabled();
            if (_panelRenderer)
            {
                _panelRenderer.RegisterUIReloadCallback(OnUIReload);
            }
            else
            {
                OnUILoad(_document.rootVisualElement);
            }
        }
        
        protected sealed override void OnDisabled()
        {
            if (_panelRenderer) _panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        }
        
        private void OnUIReload(PanelRenderer panelRenderer, VisualElement rootElement)
        {
            OnUILoad(rootElement);
        }

        private void OnUILoad(VisualElement root)
        {
            _root = root;
            OnEnableUI();
            OnViewModelSet();
        }
        
        protected abstract void OnEnableUI();
        protected abstract void OnViewModelSet();
    }
    
    public abstract class View<TViewModel> : View where TViewModel : ViewModel, new()
    {
        private int _viewId;
        private static int _viewCount;
        protected static TViewModel _viewModel;

        protected void Awake()
        {
            _viewId = _viewCount;
            _viewCount++;
            _viewModel ??= new TViewModel();
            OnAwake();
        }
        protected virtual void OnAwake() { }

        protected void OnEnable()
        {
            OnEnabled();
        }
        protected virtual void OnEnabled() { }
        
        private void Update()
        {
            if (_viewId == 0) _viewModel?.OnUpdate();
            OnUpdate();
        }
        protected virtual void OnUpdate() { }

        protected void OnDestroy()
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