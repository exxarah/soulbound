using System;
using System.Collections.Generic;
using Game.AIBehaviour.Utils;
using Game.Input;
using UnityEngine;

namespace Game.AIBehaviour.Tasks
{
    public class TaskGoToTarget : Node
    {
        private FrameInputData m_inputData;
        private Func<Transform> m_targetFunc;

        private Vector2 m_lastDirection;

        public TaskGoToTarget(ABehaviourTree tree, Func<Transform> targetFunc = null) : base(tree)
        {
            m_inputData = new FrameInputData();

            if (targetFunc == null)
            {
                targetFunc = GetTargetFromTree;
            }

            m_targetFunc = targetFunc;
        }

        private Transform GetTargetFromTree()
        {
            return (Transform)GetData("target");
        }

        public override NodeState Evaluate()
        {
            Transform target = m_targetFunc.Invoke();
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
                Vector3 targetDirection = (target.position - Tree.ControlledEntity.transform.position).normalized;
                m_inputData.MovementDirection = GetSteeredDirection(new Vector2(targetDirection.x, targetDirection.z));
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

        /// <summary>
        /// Contextually weight directions based on target, collision, preferred distance to player, etc
        /// </summary>
        /// <returns>The heighest weighted direction</returns>
        private Vector2 GetSteeredDirection(Vector2 directionToTarget)
        {
            const int steeringCastCount = 8;
            const float steeringCastLength = 1.0f;

            ContextMap map = new ContextMap(steeringCastCount);
            map.Influence(directionToTarget);
            
            // Do collision checks and apply influence based on it
            RaycastHit[] results = new RaycastHit[2];
            for (int i = 0; i < steeringCastCount; i++)
            {
                Vector2 segmentDir = map.GetDirection(i);
                if (Physics.RaycastNonAlloc(Tree.ControlledEntity.Rigidbody.position, new Vector3(segmentDir.x, 0.0f, segmentDir.y), results, steeringCastLength) > 0)
                {
                    // A collision detected. Weight against it
                    map.Influence(segmentDir, AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f), 360.0f, 1.0f, -1.0f);
                }
            }
            
            // Smoothing between this and last direction
            Vector2 direction = map.GetDirection();
            if (direction != m_lastDirection)
            {
                if (m_lastDirection != Vector2.zero && Vector2.Angle(direction, m_lastDirection) <= 1.0f)
                {
                    direction = m_lastDirection;
                }
                else
                {
                    direction = Vector2.Lerp(m_lastDirection, direction, 0.1f);   
                }
                m_lastDirection = direction;
            }

            return direction;
        }
    }
}