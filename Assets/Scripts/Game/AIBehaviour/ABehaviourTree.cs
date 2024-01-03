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

        [SerializeField, Range(0.0f, 1.0f), Tooltip("What percentage of range should we aim for (ie, 1 means an attack triggers as soon as someone is in range, 0.5 means an attack triggers once they're half way into our range)")]
        private float m_minAbilityRangeTarget = 0.7f;
        public float MinAbilityRangeTarget => m_minAbilityRangeTarget;
        
        [SerializeField, Range(0.0f, 1.0f), Tooltip("What percentage of range should we aim for (ie, 1 means an attack triggers as soon as someone is in range, 0.5 means an attack triggers once they're half way into our range)")]
        private float m_maxAbilityRangeTarget = 0.4f;
        public float MaxAbilityRangeTarget => m_maxAbilityRangeTarget;
        
        private Node m_root = null;
        public Node Root => m_root;

        private void Start()
        {
            m_root = SetupTree();
        }

        private void Update()
        {
            if (m_root == null) { return; }
            if (m_controlledEntity.HealthComponent != null && m_controlledEntity.HealthComponent.IsDead) { return; }

            m_root.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}