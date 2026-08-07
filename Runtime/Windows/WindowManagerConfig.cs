using UnityEngine;

namespace IRG.Windows
{
    public class WindowManagerConfig : MonoBehaviour
    {
        public bool ShowOnlyCurrentLevel;

        private void OnValidate()
        {
            WindowManager.ShowOnlyCurrentLevel = ShowOnlyCurrentLevel;
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}
