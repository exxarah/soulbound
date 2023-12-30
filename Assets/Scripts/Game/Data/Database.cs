using Core.Unity.Utils;
using UnityEngine;

namespace Game.Data
{
    public class Database : ScriptableObject
    {
        [SerializeField]
        private CreditsDatabase m_creditsDatabase;
        public CreditsDatabase CreditsDatabase => m_creditsDatabase;
        
        [SerializeField]
        private AbilityDatabase m_abilityDatabase;
        public AbilityDatabase AbilityDatabase => m_abilityDatabase;
        
        [SerializeField]
        private EnemyDatabase m_enemyDatabase;
        public EnemyDatabase EnemyDatabase => m_enemyDatabase;

        [SerializeField]
        private EnemyWaveDatabase m_waveDatabase;
        public EnemyWaveDatabase WaveDatabase => m_waveDatabase;
    }
}