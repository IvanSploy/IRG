using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Editor
{
    public class RefactorInAssetsWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;
        
        private VisualElement _root;
        private TextField _oldName;
        private TextField _newName;
        private Button _applyButton;

        [MenuItem("IRG Debug/Refactor Assets")]
        public static void CreateWindow()
        {
            var wnd = GetWindow<RefactorInAssetsWindow>();
            wnd.titleContent = new GUIContent("Refactor Assets");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            _root = _visualTreeAsset.CloneTree().ElementAt(0);
            root.Add(_root);
            
            //Get UXML
            _oldName = _root.Q<TextField>("OldName");
            _newName = _root.Q<TextField>("NewName");
            _applyButton = _root.Q<Button>("ApplyButton");

            _applyButton.clicked += () =>
            {
                if (string.IsNullOrEmpty(_oldName.text) || string.IsNullOrEmpty(_newName.text)) return;
                if (_oldName.text == _newName.text) return;
                AssetsRefactor.ReplaceInAssets(_oldName.text, _newName.text);
                _oldName.SetValueWithoutNotify("");
                _newName.SetValueWithoutNotify("");
            };
        }
    } 
}
