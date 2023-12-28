using Game.Input;

namespace Game.Entity
{
    public class EntityDeadState : AEntityState
    {
        public EntityDeadState(Entity stateMachine) : base(stateMachine) { }

        public override void ApplyInput(FrameInputData input)
        {
            return;
        }
    }
}