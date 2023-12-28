using System;
using UnityEngine;

namespace Game.AIBehaviour
{
    public abstract class ABehaviourTree : MonoBehaviour
    {
        [SerializeField]
        private Entity.Entity m_controlledEntity;
        public Entity.Entity ControlledEntity => m_controlledEntity;

        [SerializeField]
        private float m_fovRange = 10.0f;
        public float FOVRange => m_fovRange;
        
        private Node m_root = null;
        public Node Root => m_root;

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