using System;

namespace Game.AIBehaviour.Nodes
{
    // Basically it's an AND node
    public class SequenceNode : Node
    {
        public SequenceNode(ABehaviourTree tree) : base(tree) { }

        public override NodeState Evaluate()
        {
            bool anyChildRunning = false;
            foreach (Node node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        // Any child failed? This fails
                        State = NodeState.Failure;
                        return State;
                    case NodeState.Running:
                        anyChildRunning = true;
                        continue;
                    case NodeState.Success:
                        continue;
                    default:
                        State = NodeState.Success;
                        return State;
                }
            }

            State = anyChildRunning ? NodeState.Running : NodeState.Success;
            return State;
        }
    }
}