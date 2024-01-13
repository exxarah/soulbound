using System;
using Core.Unity.Utils;
using Game.Input;
using UnityEngine;

namespace Game.UI.Player
{
    public class PlayerInputDisplayComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_keyboardMouseControl = null;

        [SerializeField]
        private GameObject m_controllerControl = null;
        
        public void OnEnable()
        {
            m_keyboardMouseControl.SetActiveSafe(false);
            m_controllerControl.SetActiveSafe(false);
            
            switch (GameContext.Instance.InputManager.PreferredControl)
            {
                case InputManager.PreferredControls.KeyboardMouse:
                    m_keyboardMouseControl.SetActiveSafe(true);
                    break;
                case InputManager.PreferredControls.ControllerXbox:
                    m_controllerControl.SetActiveSafe(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}