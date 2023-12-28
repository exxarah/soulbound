namespace Game.AIBehaviour.Nodes
{
    // Basically it's an OR node
    public class SelectorNode : Node
    {
        public SelectorNode(ABehaviourTree tree) : base(tree) { }
        
        public override NodeState Evaluate()
        {
            foreach (Node node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        continue;
                    case NodeState.Running:
                        State = NodeState.Running;
                        return State;
                    case NodeState.Success:
                        State = NodeState.Success;
                        return State;
                    default:
                        continue;
                }
            }

            State = NodeState.Failure;
            return State;
        }
    }
}