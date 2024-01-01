using System;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public class PlayerInputComponent : MonoBehaviour, GameInputActions.IPlayerActions
    {
        [Serializable]
        private enum IsometricSkewPreference
        {
            ScreenSpace,
            UpIsLeft,
            UpIsRight,
        }
        
        [SerializeField]
        private Entity.Entity m_controlledEntity = null;

        [SerializeField]
        private Camera m_viewCamera = null;
        
        [SerializeField]
        private IsometricSkewPreference m_isometricSkewPreference = IsometricSkewPreference.ScreenSpace;

        private Matrix4x4 m_isometricSkew = Matrix4x4.identity;
        private FrameInputData m_inputData = new FrameInputData();

        private GameInputActions.PlayerActions m_playerActions;

        private void OnEnable()
        {
            SetIsometricSkew();
            GameContext.Instance.InputManager.InputActions.Player.AddCallbacks(this);
            GameContext.Instance.InputManager.InputActions.Player.Enable();
        }

        private void OnDisable()
        {
            GameContext.Instance.InputManager.InputActions.Player.Disable();
            GameContext.Instance.InputManager.InputActions.Player.RemoveCallbacks(this);
        }

        private void FixedUpdate()
        {
            if (m_controlledEntity.HealthComponent.IsDead) {return; }
            
            m_controlledEntity.ApplyInput(m_inputData);
        }
        
        private void SetIsometricSkew()
        {
            switch (m_isometricSkewPreference)
            {
                case IsometricSkewPreference.ScreenSpace:
                    m_isometricSkew = Matrix4x4.Rotate(Quaternion.Euler(0.0f, 45.0f, 0.0f));
                    break;
                case IsometricSkewPreference.UpIsLeft:
                    break;
                case IsometricSkewPreference.UpIsRight:
                    m_isometricSkew = Matrix4x4.Rotate(Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            // Transform the movement input to accommodate isometric perspective
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 movementInput = new Vector3(input.x, 0.0f, input.y);
            movementInput = m_isometricSkew.MultiplyPoint3x4(movementInput);
            m_inputData.MovementDirection = new Vector2(movementInput.x, movementInput.z);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Vector3 lookAt = Vector3.zero;
            switch (context.control.device)
            {
                case Mouse:
                    Physics.Raycast(m_viewCamera.ScreenPointToRay(context.ReadValue<Vector2>()), out RaycastHit hit);
                    lookAt = new Vector3(hit.point.x, 0.0f, hit.point.z);
                    break;
                case Gamepad:
                {
                    Vector2 input = context.ReadValue<Vector2>().normalized;
                    lookAt = m_controlledEntity.Rigidbody.transform.position + new Vector3(input.x * 10, 0.0f, input.y * 10);
                    break;
                }
            }
            Log.Info(context.control.device.GetType().ToString());
            m_controlledEntity.Rigidbody.transform.LookAt(lookAt, Vector3.up);
        }

        public void OnBasicAttack(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                m_inputData.BasicAttack = false;
            }
            else if (context.started)
            {
                m_inputData.BasicAttack = true;
            }
        }

        public void OnBasicMobility(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                m_inputData.BasicMitigation = false;
            }
            else if (context.started)
            {
                m_inputData.BasicMitigation = true;
            }
        }

        public void OnCharmAbility(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                m_inputData.CharmAbility = false;
            }
            else if (context.started)
            {
                m_inputData.CharmAbility = true;
            }
        }

        public void OnAbility1(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                m_inputData.AbilityOne = false;
            }
            else if (context.started)
            {
                m_inputData.AbilityOne = true;
            }
        }

        public void OnAbility2(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                m_inputData.AbilityTwo = false;
            }
            else if (context.started)
            {
                m_inputData.AbilityTwo = true;
            }
        }

        public void OnAbility3(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                m_inputData.AbilityThree = false;
            }
            else if (context.started)
            {
                m_inputData.AbilityThree = true;
            }
        }

        public void OnAbility4(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                m_inputData.AbilityFour = false;
            }
            else if (context.started)
            {
                m_inputData.AbilityFour = true;
            }
        }

        public void OnPause(InputAction.CallbackContext context) { }
    }
}