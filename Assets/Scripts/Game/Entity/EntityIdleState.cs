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
            AbilityDatabase.AbilityDefinition ability = null;
            if (input.CharmAbility && Database.Instance.AbilityDatabase.TryGetAbility(Entity.AbilitiesComponent.GetAbility(FrameInputData.ActionType.CharmAbility), out ability))
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, ability));
                return;
            }
            
            if (input.AbilityOne && Database.Instance.AbilityDatabase.TryGetAbility(Entity.AbilitiesComponent.GetAbility(FrameInputData.ActionType.AbilityOne), out ability))
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, ability));
                return;
            }
            
            if (input.AbilityTwo && Database.Instance.AbilityDatabase.TryGetAbility(Entity.AbilitiesComponent.GetAbility(FrameInputData.ActionType.AbilityTwo), out ability))
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, ability));
                return;
            }
            
            if (input.AbilityThree && Database.Instance.AbilityDatabase.TryGetAbility(Entity.AbilitiesComponent.GetAbility(FrameInputData.ActionType.AbilityThree), out ability))
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, ability));
                return;
            }
            
            if (input.AbilityFour && Database.Instance.AbilityDatabase.TryGetAbility(Entity.AbilitiesComponent.GetAbility(FrameInputData.ActionType.AbilityFour), out ability))
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, ability));
                return;
            }
            
            if (input.BasicAttack && Database.Instance.AbilityDatabase.TryGetAbility(Entity.AbilitiesComponent.GetAbility(FrameInputData.ActionType.BasicAttack), out ability))
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, ability));
                return;
            }
            
            if (input.MovementDirection != Vector2.zero)
            {
                StateMachine.ChangeState(new EntityMovingState(Entity));
                return;
            }
        }
    }
}