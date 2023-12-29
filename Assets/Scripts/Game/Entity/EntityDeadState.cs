using Game.Input;
using UnityEngine;

namespace Game.Entity
{
    public class EntityDeadState : AEntityState
    {
        public EntityDeadState(Entity stateMachine) : base(stateMachine) { }

        public override void FixedUpdate()
        {
            Entity.Rigidbody.velocity = Vector3.zero;
        }

        public override void ApplyInput(FrameInputData input) { }
    }
}