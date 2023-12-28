using UnityEngine;

namespace Core.DataStructure
{
    public abstract class State
    {
        protected StateMachine StateMachine;
        public State(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Exit() { }

    }

    public class StateMachine : MonoBehaviour
    {
        public State PreviousState;
        public State CurrentState;

        public virtual void Update()
        {
            CurrentState?.Update();
        }

        public virtual void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }

        public virtual void ChangeState(State newState)
        {
            CurrentState?.Exit();
            PreviousState = CurrentState;
            CurrentState = newState;
            CurrentState?.Enter();
        }
    }
}