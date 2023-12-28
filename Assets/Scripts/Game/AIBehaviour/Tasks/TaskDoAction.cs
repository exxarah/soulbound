using Game.Input;

namespace Game.AIBehaviour.Tasks
{
    /// <summary>
    /// Do basic attack
    /// </summary>
    public class TaskDoAction : Node
    {
        private FrameInputData m_inputData;

        public TaskDoAction(ABehaviourTree tree, FrameInputData.ActionType action) : base(tree)
        {
            m_inputData = new FrameInputData();
            m_inputData.SetAction(action, true);
        }

        public override NodeState Evaluate()
        {
            Tree.ControlledEntity.ApplyInput(m_inputData);
            // TODO: Lock them into this

            State = NodeState.Running;
            return State;
        }
    }
}