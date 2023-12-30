using Game.Data;
using Game.Input;
using UnityEngine;

namespace Game.Entity
{
    public class EntityIdleState : AEntityState
    {
        public EntityIdleState(Entity stateMachine) : base(stateMachine) { }

        public override void FixedUpdate()
        {
            Entity.Rigidbody.velocity = Vector3.zero;
        }

        public override void ApplyInput(FrameInputData input)
        {
            if (input.CharmAbility)
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, FrameInputData.ActionType.CharmAbility));
                return;
            }
            
            if (input.AbilityOne)
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, FrameInputData.ActionType.AbilityOne));
                return;
            }
            
            if (input.AbilityTwo)
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, FrameInputData.ActionType.AbilityTwo));
                return;
            }
            
            if (input.AbilityThree)
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, FrameInputData.ActionType.AbilityThree));
                return;
            }
            
            if (input.AbilityFour)
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, FrameInputData.ActionType.AbilityFour));
                return;
            }
            
            if (input.BasicAttack)
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, FrameInputData.ActionType.BasicAttack));
                return;
            }

            if (input.BasicMitigation)
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, FrameInputData.ActionType.BasicMitigation));
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