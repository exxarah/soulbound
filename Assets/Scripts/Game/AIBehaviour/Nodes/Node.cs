using System.Collections.Generic;

namespace Game.AIBehaviour
{
    public enum NodeState
    {
        Running,
        Success,
        Failure,
    }
    
    public class Node
    {
        protected NodeState State;

        protected Node Parent = null;
        protected List<Node> Children = new List<Node>();

        private Dictionary<string, object> m_dataContext = new();
        protected ABehaviourTree Tree;

        public Node(ABehaviourTree tree)
        {
            Tree = tree;
        }

        private void AddChild(Node child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public virtual NodeState Evaluate() => NodeState.Failure;

        public void SetData(string key, object value)
        {
            m_dataContext[key] = value;
        }

        // Check if the given data exists in this node, or any node above it
        public object GetData(string key)
        {
            if (m_dataContext.TryGetValue(key, out object value))
            {
                return value;
            }

            Node node = Parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                {
                    return value;
                }

                node = node.Parent;
            }

            return null;
        }
        
        public bool ClearData(string key)
        {
            if (m_dataContext.Remove(key))
            {
                return true;
            }

            Node node = Parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                {
                    return true;
                }

                node = node.Parent;
            }

            return false;
        }
    }
}