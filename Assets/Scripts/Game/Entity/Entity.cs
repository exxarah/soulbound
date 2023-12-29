using System;
using System.Collections.Generic;
using Core.DataStructure;
using Game.Combat;
using Game.Data;
using Game.Input;
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

        [Header("Entity Information")]
        [SerializeField]
        private float m_speed = 30.0f;
        public float Speed => m_speed;

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
        }

        public override void Update()
        {
            if (m_healthComponent != null && m_healthComponent.IsDead)
            {
                ChangeState(new EntityDeadState(this));
            }
            
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

        public void StartCooldown(AbilityDatabase.AbilityDefinition ability)
        {
            m_cooldownIDs.Add(ability);
            m_cooldownValues.Add(0.0f);
        }
    }
}