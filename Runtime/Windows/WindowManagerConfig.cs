using UnityEngine;

namespace IRG.Windows
{
    //TODO: Convert this into a Toolbar toggle using PlayerPrefs, or something more project specific.
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
