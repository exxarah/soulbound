using Core.DataStructure;
using Game.Input;

namespace Game.Entity
{
    public abstract class AEntityState : State
    {
        protected Entity Entity;

        protected AEntityState(Entity stateMachine) : base(stateMachine)
        {
            Entity = stateMachine;
        }

        public abstract void ApplyInput(FrameInputData input);
    }
}