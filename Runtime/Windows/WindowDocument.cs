using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Windows
{
    public class WindowDocument : MonoBehaviour, IWindow
    {
        [SerializeField] private bool _startOpened;
        [SerializeField] private string _name;
        public UIDocument Document;

        private VisualElement _root;

        public string Name => _name;
        public bool IsOpen { get; private set; }

        private void Awake()
        {
            IsOpen = _startOpened;
            _root = Document.rootVisualElement;
            _root.SetDisplay(_startOpened);
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
