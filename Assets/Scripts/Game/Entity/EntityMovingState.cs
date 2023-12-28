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

            m_movementInput = input.MovementDirection;
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            Vector3 direction = new Vector3(m_movementInput.x, 0.0f, m_movementInput.y).normalized;
            Entity.CharacterController.SimpleMove(direction * Entity.Speed * Time.deltaTime);
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
        }
    }
}