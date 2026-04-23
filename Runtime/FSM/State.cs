namespace IRG.FSM
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Exit();
        public virtual void Update() {}
        public virtual void FixedUpdate() {}
        public virtual void LateUpdate() {}
    }
}