using System;
using System.Collections.Generic;

namespace Game.AIBehaviour.Conditionals
{
    public class IsTrue : Node
    {
        private Func<bool> m_toCheck = null;

        public IsTrue(ABehaviourTree tree, Func<bool> toCheck) : base(tree)
        {
            m_toCheck = toCheck;
        }

        public IsTrue(ABehaviourTree tree, List<Node> children, Func<bool> toCheck) : base(tree, children)
        {
            m_toCheck = toCheck;
        }

        public override NodeState Evaluate()
        {
            return m_toCheck.Invoke() ? NodeState.Success : NodeState.Failure;
        }
    }
}