using Game.Input;

namespace Game.AIBehaviour.Tasks
{
    public class TaskIdle : Node
    {
        private FrameInputData m_inputData;
        
        public TaskIdle(ABehaviourTree tree) : base(tree)
        {
            m_inputData = new FrameInputData();
        }

        public override NodeState Evaluate()
        {
            Tree.ControlledEntity.ApplyInput(m_inputData);
            
            State = NodeState.Running;
            return State;
        }
    }
}