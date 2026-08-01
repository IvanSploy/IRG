using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Windows
{
    public class WindowDocument : MonoBehaviour, IWindow
    {
        [field:SerializeField] public bool IsOpen { get; private set; }
        [SerializeField] private string _name;
        [field:SerializeField] public int Level { get; private set; }
        public UIDocument Document;

        private VisualElement _root;

        public string Name => _name;

        private void Awake()
        {
            _root = Document.rootVisualElement;
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
