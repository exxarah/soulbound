using System;
using UnityEngine;

namespace Game.Input
{
    public class PlayerInputComponent : MonoBehaviour
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

        private void OnEnable()
        {
            SetIsometricSkew();
        }

        private void FixedUpdate()
        {
            // Transform the movement input to accommodate isometric perspective
            Vector3 movementInput = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0.0f, UnityEngine.Input.GetAxis("Vertical"));
            movementInput = m_isometricSkew.MultiplyPoint3x4(movementInput);
            m_inputData.MovementDirection = new Vector2(movementInput.x, movementInput.z);

            m_inputData.BasicAttack = UnityEngine.Input.GetMouseButtonDown(0);
            m_inputData.Shield = UnityEngine.Input.GetMouseButtonDown(1);

            m_inputData.CharmAbility = UnityEngine.Input.GetKeyDown(KeyCode.Space);
            
            m_controlledEntity.ApplyInput(m_inputData);

            // Player looks towards the mouse, not movement direction
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            Physics.Raycast(m_viewCamera.ScreenPointToRay(UnityEngine.Input.mousePosition), out RaycastHit hit);
            m_controlledEntity.Rigidbody.transform.LookAt(new Vector3(hit.point.x, 0.0f, hit.point.z), Vector3.up);
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
    }
}