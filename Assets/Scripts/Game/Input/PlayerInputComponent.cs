using UnityEngine;

namespace Game.Input
{
    public class PlayerInputComponent : MonoBehaviour
    {
        [SerializeField]
        private Entity.Entity m_controlledEntity = null;
        
        private FrameInputData m_inputData = new FrameInputData();

        private void Update()
        {
            m_inputData.MovementDirection = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
            
            m_controlledEntity.ApplyInput(m_inputData);
        }
    }
}