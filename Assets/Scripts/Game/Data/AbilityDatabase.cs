using System;
using System.Collections.Generic;
using PracticeJam.Game.Combat;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu]
    public class AbilityDatabase : ScriptableObject
    {
        [Serializable]
        public class AbilityDefinition
        {
            [SerializeField]
            private string m_abilityID;
            public string AbilityID => m_abilityID;

            [SerializeField]
            private string m_animationName;
            public string AnimationName => m_animationName;

            [SerializeField]
            private Enums.TargetType m_targetType;
            public Enums.TargetType TargetType => m_targetType;

            [SerializeField]
            private float m_attackRange = 1.0f;
            public float AttackRange => m_attackRange;

            [SerializeField]
            private float m_coneDegrees = 30.0f;
            public float ConeDegrees => m_coneDegrees;

            [SerializeField]
            private int m_maxTargets = 1;
            public int MaxTargets => m_maxTargets;
        }
        
        [SerializeField]
        private List<AbilityDefinition> m_definitions = new List<AbilityDefinition>();
        public IReadOnlyList<AbilityDefinition> Definitions => m_definitions;

        public AbilityDefinition GetAbility(string abilityID)
        {
            return m_definitions.Find((definition => definition.AbilityID == abilityID));
        }

        public bool TryGetAbility(string abilityID, out AbilityDefinition ability)
        {
            ability = GetAbility(abilityID);
            return ability != null;
        }
    }
}