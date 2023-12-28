using System;
using UnityEngine;

namespace Game.AIBehaviour
{
    public abstract class ABehaviourTree : MonoBehaviour
    {
        [SerializeField]
        private Entity.Entity m_controlledEntity;
        public Entity.Entity ControlledEntity => m_controlledEntity;
        
        private Node m_root = null;

        private void Start()
        {
            m_root = SetupTree();
        }

        private void Update()
        {
            if (m_root == null) { return; }

            m_root.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}