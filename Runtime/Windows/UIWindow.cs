using UnityEngine.UIElements;

namespace IRG.Windows
{
    public class UIWindow : Window
    {
        private PanelRenderer _panelRenderer;
        private UIDocument _document;
        private VisualElement _root;

        private void Awake()
        {
            _panelRenderer = GetComponent<PanelRenderer>();
            _document = GetComponent<UIDocument>();
        }

        void OnEnable()
        {
            if (_panelRenderer)
            {
                _panelRenderer.RegisterUIReloadCallback(OnUIReload);
            }
            else
            {
                Initialize(_document.rootVisualElement);
            }
        }
        
        void OnDisable()
        {
            if(_panelRenderer) _panelRenderer.UnregisterUIReloadCallback(OnUIReload);
            UnRegister();
        }
        
        void OnUIReload(PanelRenderer renderer, VisualElement rootElement)
        {
            Initialize(rootElement);
        }

        private void Initialize(VisualElement rootElement)
        {
            _root = rootElement;
            _root.SetDisplay(IsOpen);
            Register();
        }

        public override void Open()
        {
            IsOpen = true;
            _root.SetDisplay(true);
        }

        public override void Close()
        {
            IsOpen = false;
            _root.SetDisplay(false);
        }
    }
}
