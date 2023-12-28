using Core.Unity.Utils;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu]
    public class Database : ScriptableObjectSingleton<Database>
    {
        [SerializeField]
        private AbilityDatabase m_abilityDatabase;
        public AbilityDatabase AbilityDatabase => m_abilityDatabase;
    }
}