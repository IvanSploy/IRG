using UnityEngine;

namespace IRG
{
    public abstract class Singleton<TSingleton> : MonoBehaviour where TSingleton : Singleton<TSingleton>
    {
        public static TSingleton Instance;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                return;
            }

            Instance = (TSingleton)this;
            OnAwake();
        }
        protected virtual void OnAwake() { }
    }
}