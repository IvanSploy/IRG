using System;

namespace IRG
{
    public class ReactiveCommand<T>
    {
        private event Action<T> NotifyEvent;

        public ReactiveCommand() { }

        public void Execute(T parameter)
        {
            NotifyEvent?.Invoke(parameter);
        }
        
        public ActionDisposable Subscribe(Action<T> action)
        {
            NotifyEvent += action;
            return new ActionDisposable(() => UnSubscribe(action));
        }
        
        public void UnSubscribe(Action<T> action)
        {
            NotifyEvent -= action;
        }
    }

    public class ReactiveCommand
    {
        private event Action NotifyEvent;

        public ReactiveCommand() { }
        
        public void Execute()
        {
            NotifyEvent?.Invoke();
        }

        public ActionDisposable Subscribe(Action action)
        {
            NotifyEvent += action;
            return new ActionDisposable(() => UnSubscribe(action));
        }
        
        public void UnSubscribe(Action action)
        {
            NotifyEvent -= action;
        }
    }
}