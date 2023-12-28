using System;
using Core.DataStructure;
using Game.Input;
using UnityEngine;

namespace Game.Entity
{
    public class Entity : StateMachine
    {
        [SerializeField]
        private CharacterController m_characterController = null;
        public CharacterController CharacterController => m_characterController;

        [SerializeField]
        private Animator m_animator = null;
        public Animator Animator => m_animator;

        [Header("Entity Stats")]
        [SerializeField]
        private float m_speed = 30.0f;
        public float Speed => m_speed;

        private void Start()
        {
            ChangeState(new EntityIdleState(this));
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