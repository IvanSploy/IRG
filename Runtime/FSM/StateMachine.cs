using System;
using System.Collections.Generic;

namespace IRG.FSM
{
    public class StateMachine
    {
        public bool Active { get; set; } = true;
        public State DefaultState { get; private set; }
        public State CurrentState { get; private set; }
        
        protected readonly Dictionary<Type, State> States = new();

        public void Initialize(State initialState)
        {
            DefaultState = initialState;
            CurrentState = initialState;
            initialState.Enter();
        }

        public bool TryGetState<T>(out T state) where T : State
        {
            state = GetState<T>();
            return state != null;
        }

        public T GetState<T>() where T : State
        {
            if (States.TryGetValue(typeof(T), out var state)) return (T)state;
            return null;
        }
        
        public void AddState<T>(T state) where T : State
        {
            States.Add(typeof(T), state);
        }

        public void RemoveState<T>() where T : State
        {
            if(CurrentState.GetType() == typeof(T)) ChangeState(DefaultState);
            States.Remove(typeof(T));
        }

        public void RemoveState(State state)
        {
            if(CurrentState == state) ChangeState(DefaultState);
            States.Remove(state.GetType());
        }

        public bool ChangeState<T>()
        {
            if (!States.ContainsKey(typeof(T))) return false;
            ChangeState(States[typeof(T)]);
            return true;
        }
        
        private void ChangeState(State state)
        {
            CurrentState.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }
        
        public void CheckTransitions()
        {
            if (!Active) return;
            OnCheckTransitions();
        }
        
        public virtual void OnCheckTransitions(){}

        public void Update()
        {
            if (!Active) return;
            CurrentState?.Update();
        }
        
        public void FixedUpdate()
        {
            if (!Active) return;
            CurrentState?.FixedUpdate();
        }
        
        public void LateUpdate()
        {
            if (!Active) return;
            CurrentState?.LateUpdate();
        }
    }
}