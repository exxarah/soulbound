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

            m_telegraphSeconds = Random.Range(tree.TelegraphMinSeconds, tree.TelegraphMaxSeconds);
        }

        public override NodeState Evaluate()
        {
            if (Tree.ControlledEntity.IsOnCooldown(Tree.ControlledEntity.AbilitiesComponent.GetAbility(m_action)))
            {
                State = NodeState.Failure;
                return State;
            }

            object inProgress = Tree.Root.GetData("basic_ability_in_progress");
            if (inProgress == null || !(bool)inProgress)
            {
                AbilityDatabase.AbilityDefinition abilityDef =
                    Database.Instance.AbilityDatabase.GetAbility(Tree.ControlledEntity.AbilitiesComponent
                                                                     .GetAbility(m_action));
                m_inputData.SetAction(m_action, true);
                Tree.ControlledEntity.ApplyInput(m_inputData);
                m_secondsSinceStart = 0.0f;

                if (abilityDef.HasTelegraph)
                {
                    // Lock them into this
                    Tree.Root.SetData("basic_ability_in_progress", true);
                    State = NodeState.Running;   
                }
                else
                {
                    Tree.Root.ClearData("basic_ability_in_progress");
                    State = NodeState.Success;
                }
            }
            else
            {
                // Currently in progress
                m_secondsSinceStart += Time.deltaTime;
                if (m_secondsSinceStart >= m_telegraphSeconds)
                {
                    // Finished telegraphing. Execute!
                    m_inputData.SetAction(m_action, false);
                    Tree.ControlledEntity.ApplyInput(m_inputData);
                    Tree.Root.ClearData("basic_ability_in_progress");
                }
            }
            return State;
        }
    }
}