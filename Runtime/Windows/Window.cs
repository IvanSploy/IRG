using UnityEngine;
using UnityEngine.Serialization;

namespace IRG.Windows
{
    public abstract class Window : MonoBehaviour
    {
        [field:FormerlySerializedAs("_name")]
        [field:SerializeField] public string Key { get; protected set; }
        [field:SerializeField] public int Level { get; protected set; }
        [field:SerializeField] public bool IsOpen { get; protected set; }
        [field:SerializeField] public bool ShowAlwaysWhenOpen { get; protected set; }

        protected void Register()
        {
            WindowManager.RegisterWindow(this);
        }

        protected void UnRegister()
        {
            WindowManager.UnRegisterWindow(this);
        }

        public abstract void Open();
        public abstract void Close();
    }
}