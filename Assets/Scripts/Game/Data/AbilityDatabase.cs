using System;
using System.Collections.Generic;
using Game.Combat;
using Game.Combat.Effects;
using UnityEngine;

namespace Game.Data
{
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
            private CharmCost m_charmCost;
            public CharmCost CharmCost => m_charmCost;

            [SerializeField]
            private int m_cooldownSeconds = 6;
            public int CooldownSeconds => m_cooldownSeconds;

            [SerializeField]
            private Sprite m_uiImage = null;
            public Sprite UIImage => m_uiImage;

            [Header("Targeting Information")]
            [SerializeField]
            private Enums.TargetType m_targetType;
            public Enums.TargetType TargetType => m_targetType;

            [SerializeField]
            private Enums.TargetLayer m_targetLayer;
            public Enums.TargetLayer TargetLayer => m_targetLayer;

            [SerializeField]
            private float m_attackRange = 1.0f;
            public float AttackRange => m_attackRange;

            [SerializeField]
            private float m_coneDegrees = 30.0f;
            public float ConeDegrees => m_coneDegrees;

            [SerializeField]
            private int m_maxTargets = 1;
            public int MaxTargets => m_maxTargets;

            [Header("Result Information")]
            [SerializeField]
            private int m_healthDecrement = 0;
            public int HealthDecrement => m_healthDecrement;

            [SerializeField]
            private bool m_instaKill = false;
            public bool InstaKill => m_instaKill;
            
            [SerializeField]
            private List<AAbilityEffect> m_effects = new List<AAbilityEffect>();
            public IReadOnlyList<AAbilityEffect> Effects => m_effects;

            public List<AAbilityEffect> GetEffects(Func<AAbilityEffect, bool> matchesFunc)
            {
                return m_effects.FindAll((effect => matchesFunc.Invoke(effect)));
            }
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