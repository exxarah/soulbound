using System.Collections.Generic;
using Game.AIBehaviour.Conditionals;
using Game.AIBehaviour.Nodes;
using Game.AIBehaviour.Tasks;
using UnityEngine;

namespace Game.AIBehaviour
{
    public class FollowPlayerTree : ABehaviourTree
    {
        [SerializeField]
        private float m_minDistanceToPlayer = 1.5f;

        [SerializeField]
        private float m_maxDistanceToPlayer = 3.0f;

        // How close the AI wants to be to the player
        private float m_idealDistanceToPlayer;

        private Transform m_toFollow = null;

        protected override Node SetupTree()
        {
            m_idealDistanceToPlayer = Random.Range(m_minDistanceToPlayer, m_maxDistanceToPlayer);

            Node root = new SelectorNode(this, new List<Node>()
            {
                // If no target to follow, idle
                new SequenceNode(this, new List<Node>()
                {
                    new IsTrue(this, () => m_toFollow == null),
                    new TaskIdle(this),
                }),
                // If within min/max distance to player, idle
                new SequenceNode(this, new List<Node>()
                {
                    new IsTrue(this, WithinRange),
                    new TaskIdle(this),
                }),
                // Go to ideal distance
                new TaskGoToTarget(this, () => m_toFollow),
                // Fallback, idle
                new TaskIdle(this),
            });

            return root;
        }

        public void SetToFollow(Transform transform)
        {
            m_toFollow = transform;
        }

        private bool WithinRange()
        {
            float distance = Vector3.Distance(m_toFollow.position, ControlledEntity.transform.position);
            return m_minDistanceToPlayer <= distance && distance <= m_maxDistanceToPlayer;
        }
    }
}