using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Windows
{
    [RequireComponent(typeof(PanelRenderer))]
    public class WindowPanel : MonoBehaviour, IWindow
    {
        [field:SerializeField] public bool IsOpen { get; private set; }
        [SerializeField] private string _name;
        [field:SerializeField] public int Level { get; private set; }

        private PanelRenderer _panelRenderer;
        private VisualElement _root;

        public string Name => _name;

        private void Awake()
        {
            _panelRenderer = GetComponent<PanelRenderer>();
        }

        void OnEnable()
        {
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }
        
        void OnDisable()
        {
            _panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        }
        
        void OnUIReload(PanelRenderer renderer, VisualElement rootElement)
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
