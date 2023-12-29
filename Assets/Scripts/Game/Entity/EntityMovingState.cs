using Game.Data;
using Game.Input;
using UnityEngine;

namespace Game.Entity
{
    public class EntityMovingState : AEntityState
    {
        private Vector2 m_movementInput;
        
        public EntityMovingState(Entity stateMachine) : base(stateMachine) { }

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
            
            if (input.MovementDirection == Vector2.zero)
            {
                StateMachine.ChangeState(new EntityIdleState(Entity));
                return;
            }

            m_movementInput = input.MovementDirection;
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            Vector3 direction = new Vector3(m_movementInput.x, 0.0f, m_movementInput.y).normalized;
            Entity.Rigidbody.velocity = direction * Entity.Speed * Time.deltaTime;

            if (Entity.FacesMovementDirection && direction != Vector3.zero)
            {
                Entity.Rigidbody.rotation = Quaternion.LookRotation(direction, Entity.Rigidbody.transform.up);
            }
        }

        public override void Enter()
        {
            base.Enter();
            
            Entity.Animator.SetBool("isWalking", true);
        }

        public override void Exit()
        {
            base.Exit();

            Entity.Animator.SetBool("isWalking", false);
            Entity.Rigidbody.velocity = Vector3.zero;
        }
    }
}