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
            private float m_cooldownSeconds = 6;
            public float CooldownSeconds => m_cooldownSeconds;

            [SerializeField]
            private Sprite m_uiImage = null;
            public Sprite UIImage => m_uiImage;

            [Header("Telegraphing Information")]
            [SerializeField]
            private bool m_hasTelegraph = false;
            public bool HasTelegraph => m_hasTelegraph;

            [SerializeField]
            private Telegraph m_telegraphPrefab = null;
            public Telegraph TelegraphPrefab => m_telegraphPrefab;

            [SerializeField]
            private Material m_telegraphMaterial = null;
            public Material TelegraphMaterial => m_telegraphMaterial;

            [SerializeField]
            private float m_telegraphMinimumSeconds;
            public float TelegraphMinimumSeconds => m_telegraphMinimumSeconds;

            [SerializeField]
            private float m_telegraphMaximumSeconds;
            public float TelegraphMaximumSeconds => m_telegraphMaximumSeconds;

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

            [SerializeField, Range(0.0f, 1.0f)]
            private float m_targetMinHealth = 0.0f;
            public float TargetMinHealth => m_targetMinHealth;

            [SerializeField, Range(0.0f, 1.0f)]
            private float m_targetMaxHealth = 1.0f;
            public float TargetMaxHealth => m_targetMaxHealth;

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