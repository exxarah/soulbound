using System;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public struct CharmCost
    {
        [SerializeField]
        private int m_omniCost;
        public int OmniCost => m_omniCost;

        [SerializeField]
        private int m_healthCost;
        public int HealthCost => m_healthCost;

        [SerializeField]
        private int m_attackCost;
        public int AttackCost => m_attackCost;
    }
}