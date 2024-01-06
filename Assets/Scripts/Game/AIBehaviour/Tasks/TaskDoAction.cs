using Game.Data;
using Game.Input;
using UnityEngine;

namespace Game.AIBehaviour.Tasks
{
    /// <summary>
    /// Do basic attack
    /// </summary>
    public class TaskDoAction : Node
    {
        private float m_telegraphSeconds = 0.0f;
        private float m_secondsSinceStart = 0.0f;
        private FrameInputData.ActionType m_action;
        private FrameInputData m_inputData;

        public TaskDoAction(ABehaviourTree tree, FrameInputData.ActionType action) : base(tree)
        {
            m_action = action;
            m_inputData = new FrameInputData();
            m_inputData.SetAction(action, true);
        }

        public override NodeState Evaluate()
        {
            if (Tree.ControlledEntity.IsOnCooldown(m_action))
            {
                State = NodeState.Failure;
                return State;
            }

            object inProgress = Tree.Root.GetData("action_in_progress");
            // A different action is in progress, fail
            if (inProgress != null && (FrameInputData.ActionType)inProgress != m_action)
            {
                State = NodeState.Failure;
                return NodeState.Failure;
            }
            // No action is in progress, start this one
            if (inProgress == null)
            {
                AbilityDatabase.AbilityDefinition abilityDef =
                    GameContext.Instance.Database.AbilityDatabase.GetAbility(Tree.ControlledEntity.AbilitiesComponent
                                                                     .GetAbility(m_action));
                
                // Pick a random bounds within the ability range that we're targeting, and cut off anyone outside that
                float shrunkRange = abilityDef.AttackRange * Random.Range(Tree.MinAbilityRangeTarget, Tree.MaxAbilityRangeTarget);
                Transform target = (Transform)GetData("target");
                if (Vector3.Distance(Tree.ControlledEntity.Rigidbody.transform.position, target.position) > shrunkRange)
                {
                    // Fall through to getting closer to them
                    State = NodeState.Failure;
                    return State;
                }
                
                m_telegraphSeconds = Random.Range(abilityDef.TelegraphMinimumSeconds, abilityDef.TelegraphMaximumSeconds);
                m_inputData.SetAction(m_action, true);
                Tree.ControlledEntity.ApplyInput(m_inputData);
                m_secondsSinceStart = 0.0f;

                if (abilityDef.HasTelegraph)
                {
                    // Lock them into this
                    Tree.Root.SetData("action_in_progress", m_action);
                    State = NodeState.Running;
                }
                else
                {
                    Tree.Root.ClearData("action_in_progress");
                    State = NodeState.Success;
                }
            }
            else
            {
                // Currently in progress
                m_secondsSinceStart += Time.deltaTime;
                
                // Turn to face player
                Transform target = (Transform)GetData("target");

                Vector3 lookAt = Vector3.Lerp(Tree.ControlledEntity.Rigidbody.transform.forward,
                                              (target.position - Tree.ControlledEntity.Rigidbody.transform.position)
                                             .normalized, Tree.LookSpeed * Time.deltaTime);
                Tree.ControlledEntity.Rigidbody.transform.LookAt(Tree.ControlledEntity.Rigidbody.position + lookAt, Vector3.up);
                
                if (m_secondsSinceStart >= m_telegraphSeconds)
                {
                    // Finished telegraphing. Execute!
                    m_inputData.SetAction(m_action, false);
                    Tree.ControlledEntity.ApplyInput(m_inputData);
                    Tree.Root.ClearData("action_in_progress");
                    State = NodeState.Success;
                }
                else
                {
                    State = NodeState.Running;
                }
            }
            return State;
        }
    }
}