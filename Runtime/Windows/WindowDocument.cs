using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Windows
{
    [RequireComponent(typeof(UIDocument))]
    public class WindowDocument : Window
    {
        private UIDocument _document;
        private VisualElement _root;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _root = _document.rootVisualElement;
            _root.SetDisplay(IsOpen);
            Register();
        }

        private void OnDisable()
        {
            UnRegister();
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
