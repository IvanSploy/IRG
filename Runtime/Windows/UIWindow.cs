using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Windows
{
    public class UIWindow : MonoBehaviour, IWindow
    {
        [field:SerializeField] public bool IsOpen { get; private set; }
        [SerializeField] private string _name;
        [field:SerializeField] public int Level { get; private set; }

        private PanelRenderer _panelRenderer;
        private UIDocument _document;
        private VisualElement _root;

        public string Name => _name;

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
        }
        
        void OnUIReload(PanelRenderer renderer, VisualElement rootElement)
        {
            Initialize(rootElement);
        }

        private void Initialize(VisualElement rootElement)
        {
            _root = rootElement;
            _root.SetDisplay(IsOpen);
        }

        public void Open()
        {
            IsOpen = true;
            _root.SetDisplay(true);
        }

        public void Close()
        {
            IsOpen = false;
            _root.SetDisplay(false);
        }
    }
}
