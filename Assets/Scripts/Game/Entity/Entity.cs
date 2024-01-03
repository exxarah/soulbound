using System;
using System.Collections.Generic;
using Core.DataStructure;
using Game.Combat;
using Game.Data;
using Game.Input;
using Game.Toy;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Entity
{
    public class Entity : StateMachine
    {
        [FormerlySerializedAs("m_characterController")]
        [SerializeField]
        private Rigidbody m_rigidbody = null;
        public Rigidbody Rigidbody => m_rigidbody;

        [SerializeField]
        private Animator m_animator = null;
        public Animator Animator => m_animator;

        [SerializeField]
        private bool m_isPlayer = false;
        public bool IsPlayer => m_isPlayer;

        [SerializeField, Tooltip("The position that any relevant UI should be spawned (normally above the entity's head)")]
        private Transform m_uiPosition = null;
        public Transform UIPosition => m_uiPosition;

        [Header("Entity Information")]
        [SerializeField]
        private EntityStatsComponent m_entityStatsComponent = null;
        public EntityStatsComponent Stats => m_entityStatsComponent;

        [SerializeField]
        private bool m_facesMovement = true;
        public bool FacesMovementDirection => m_facesMovement;

        [SerializeField]
        private InventoryComponent m_inventoryComponent = null;
        public InventoryComponent InventoryComponent => m_inventoryComponent;
        
        [SerializeField]
        private EquippedAbilitiesComponent m_abilitiesComponent = null;
        public EquippedAbilitiesComponent AbilitiesComponent => m_abilitiesComponent;

        [SerializeField]
        private HealthComponent m_healthComponent = null;
        public HealthComponent HealthComponent => m_healthComponent;
   
        [SerializeField]
        private LayerMask m_enemiesLayer;
        public LayerMask EnemiesLayer => m_enemiesLayer;

        [SerializeField]
        private LayerMask m_alliesLayer;
        public LayerMask AlliesLayer => m_alliesLayer;

        private List<AbilityDatabase.AbilityDefinition> m_cooldownIDs = new List<AbilityDatabase.AbilityDefinition>();
        private List<float> m_cooldownValues = new List<float>();

        private void Start()
        {
            ChangeState(new EntityIdleState(this));
            if (m_healthComponent != null)
            {
                m_healthComponent.OnDead += OnEntityDied;   
            }
        }

        private void OnDestroy()
        {
            if (m_healthComponent != null)
            {
                m_healthComponent.OnDead -= OnEntityDied;   
            }
        }

        private void OnEntityDied()
        {
            gameObject.layer = LayerMask.NameToLayer(Layers.DEAD);
            ChangeState(new EntityDeadState(this));
        }

        public override void Update()
        {
            // Tick the ability cooldowns
            for (int i = m_cooldownIDs.Count - 1; i >= 0; i--)
            {
                m_cooldownValues[i] += Time.deltaTime;
                if (m_cooldownValues[i] >= m_cooldownIDs[i].CooldownSeconds)
                {
                    m_cooldownValues.RemoveAt(i);
                    m_cooldownIDs.RemoveAt(i);
                }
            }

            base.Update();
        }

        public void ApplyInput(FrameInputData frameInput)
        {
            if (CurrentState != null)
            {
                ((AEntityState)CurrentState).ApplyInput(frameInput);
            }
        }

        public bool IsOnCooldown(string abilityID)
        {
            for (int i = 0; i < m_cooldownIDs.Count; i++)
            {
                if (m_cooldownIDs[i].AbilityID == abilityID)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsOnCooldown(AbilityDatabase.AbilityDefinition ability)
        {
            for (int i = 0; i < m_cooldownIDs.Count; i++)
            {
                if (m_cooldownIDs[i].AbilityID == ability.AbilityID)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsOnCooldown(FrameInputData.ActionType action)
        {
            string ability = m_abilitiesComponent.GetAbility(action);
            for (int i = 0; i < m_cooldownIDs.Count; i++)
            {
                if (m_cooldownIDs[i].AbilityID == ability)
                {
                    return true;
                }
            }

            return false;
        }

        public void StartCooldown(AbilityDatabase.AbilityDefinition ability)
        {
            m_cooldownIDs.Add(ability);
            m_cooldownValues.Add(0.0f);
        }

        public float GetCooldown(AbilityDatabase.AbilityDefinition abilityDef)
        {
            for (int i = 0; i < m_cooldownIDs.Count; i++)
            {
                if (m_cooldownIDs[i].AbilityID == abilityDef.AbilityID)
                {
                    return 1.0f - m_cooldownValues[i] / abilityDef.CooldownSeconds;
                }
            }

            return 0.0f;
        }
    }
}