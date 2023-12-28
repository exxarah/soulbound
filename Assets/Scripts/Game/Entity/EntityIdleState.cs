using Game.Data;
using Game.Input;
using UnityEngine;

namespace Game.Entity
{
    public class EntityIdleState : AEntityState
    {
        public EntityIdleState(Entity stateMachine) : base(stateMachine) { }

        public override void ApplyInput(FrameInputData input)
        {
            if (input.MovementDirection != Vector2.zero)
            {
                StateMachine.ChangeState(new EntityMovingState(Entity));
            }
            
            if (input.BasicAttack && Database.Instance.AbilityDatabase.TryGetAbility(Entity.AbilitiesComponent.GetAbility(FrameInputData.ActionType.BasicAttack), out AbilityDatabase.AbilityDefinition ability))
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, ability));
            }
        }
    }
}