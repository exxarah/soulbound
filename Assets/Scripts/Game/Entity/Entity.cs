using System;
using Core.DataStructure;
using Game.Combat;
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
        private EquippedAbilitiesComponent m_abilitiesComponent = null;
        public EquippedAbilitiesComponent AbilitiesComponent => m_abilitiesComponent;

        [SerializeField]
        private HealthComponent m_healthComponent = null;
        public HealthComponent HealthComponent => m_healthComponent;

        [SerializeField]
        private LayerMask m_entitiesToAttack;
        public LayerMask EntitiesToAttack => m_entitiesToAttack;

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

            base.Update();
        }

        public void ApplyInput(FrameInputData frameInput)
        {
            if (CurrentState != null)
            {
                ((AEntityState)CurrentState).ApplyInput(frameInput);   
            }
        }
    }
}