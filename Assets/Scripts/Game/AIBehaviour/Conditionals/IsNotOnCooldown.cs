using System.Collections.Generic;
using Game.Input;

namespace Game.AIBehaviour.Conditionals
{
    public class IsNotOnCooldown : Node
    {
        private FrameInputData.ActionType m_actionType;
        
        public IsNotOnCooldown(ABehaviourTree tree, FrameInputData.ActionType action) : base(tree)
        {
            m_actionType = action;
        }

        public IsNotOnCooldown(ABehaviourTree tree, List<Node> children, FrameInputData.ActionType action) : base(tree, children)
        {
            m_actionType = action;
        }

        public override NodeState Evaluate()
        {
            return Tree.ControlledEntity.IsOnCooldown(m_actionType) ? NodeState.Failure : NodeState.Success;
        }
    }
}