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
            if (input.MovementDirection == Vector2.zero)
            {
                StateMachine.ChangeState(new EntityIdleState(Entity));
                return;
            }

            if (input.BasicAttack && Database.Instance.AbilityDatabase.TryGetAbility(Entity.AbilitiesComponent.GetAbility(FrameInputData.ActionType.BasicAttack), out AbilityDatabase.AbilityDefinition ability))
            {
                StateMachine.ChangeState(new EntityAttackState(Entity, ability));
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