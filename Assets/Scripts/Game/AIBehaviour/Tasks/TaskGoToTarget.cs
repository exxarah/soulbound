using System.Collections.Generic;
using Game.Input;
using UnityEngine;

namespace Game.AIBehaviour.Tasks
{
    public class TaskGoToTarget : Node
    {
        private FrameInputData m_inputData;

        public TaskGoToTarget(ABehaviourTree tree) : base(tree)
        {
            m_inputData = new FrameInputData();
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            float distanceToTarget = Vector3.Distance(Tree.ControlledEntity.transform.position, target.position);

            if (distanceToTarget > Tree.FOVRange)
            {
                // Out of range, clear target and bail
                ClearData("target");
                State = NodeState.Failure;
                return State;
            }
            else if (distanceToTarget > 1.0f)
            {
                // Currently navigating to target
                Vector3 direction = (target.position - Tree.ControlledEntity.transform.position).normalized;
                m_inputData.MovementDirection = new Vector2(direction.x, direction.z);
                Tree.ControlledEntity.ApplyInput(m_inputData);

                State = NodeState.Running;
                return State;
            }

            // Stop moving, reached target
            m_inputData.MovementDirection = Vector2.zero;
            Tree.ControlledEntity.ApplyInput(m_inputData);

            State = NodeState.Success;
            return State;
        }
    }
}