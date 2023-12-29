using Core.Unity.Utils;
using UnityEngine;

namespace Game.Data
{
    public class Database : ScriptableObjectSingleton<Database>
    {
        [SerializeField]
        private AbilityDatabase m_abilityDatabase;
        public AbilityDatabase AbilityDatabase => m_abilityDatabase;
        
        [SerializeField]
        private EnemyDatabase m_enemyDatabase;
        public EnemyDatabase EnemyDatabase => m_enemyDatabase;
    }
}