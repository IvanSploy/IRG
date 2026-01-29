using UnityEngine;
using UnityEngine.Events;

namespace IRG
{
    public class SignalEvent : MonoBehaviour, ISignalReceiver
    {
        [SerializeField] private string _signalName;
        public UnityEvent OnSignalReceivedEvent;

        public void OnSignalReceived(Signal signal)
        {
            if(signal.Name != _signalName) return;
            OnSignalReceivedEvent?.Invoke();
        }
    }
}