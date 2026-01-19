using UnityEngine;

namespace IRG
{
    public class SignalEmitter : MonoBehaviour
    {
        private ISignalReceiver[] _listeners;
        
        private void Awake()
        {
            _listeners = GetComponentsInChildren<ISignalReceiver>();
        }

        public void SendSignal(Signal signal)
        {
            foreach (var listener in _listeners)
            {
                listener.OnSignalReceived(signal);
            }
        }
    }
}