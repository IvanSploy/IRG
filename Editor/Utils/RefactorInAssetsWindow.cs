using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Editor
{
    public class RefactorInAssetsWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;
        
        private VisualElement _root;

        private class AssetInfoView
        {
            public VisualElement Root;
            public TextField Name;
            public TextField NameSpace;
            public TextField Assembly;

            public AssetInfoView(VisualElement root)
            {
                Root = root;
                Name = Root.Q<TextField>("Name");
                NameSpace = Root.Q<TextField>("NameSpace");
                Assembly = Root.Q<TextField>("Assembly");
            }

            public AssetsRefactor.AssetInfo GetInfo()
            {
                return new AssetsRefactor.AssetInfo
                {
                    Name = Name.text,
                    NameSpace = NameSpace.text,
                    Assembly = Assembly.text
                };
            }
        }
        private AssetInfoView _oldInfoView;
        private AssetInfoView _newInfoView;
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

            _oldInfoView = new AssetInfoView(_root.Q("OldContainer"));
            _newInfoView = new AssetInfoView(_root.Q("NewContainer"));
            
            _applyButton = _root.Q<Button>("ApplyButton");

            _applyButton.clicked += () =>
            {
                AssetsRefactor.ReplaceInAssets(_oldInfoView.GetInfo(), _newInfoView.GetInfo());
            };
        }
    } 
}
