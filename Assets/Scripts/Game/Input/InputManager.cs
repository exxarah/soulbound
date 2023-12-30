using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Input
{
    public class InputManager : MonoBehaviour
    {
        [Serializable]
        public enum PreferredControls
        {
            KeyboardMouse,
            Controller,
        }
        
        [SerializeField]
        private EventSystem m_eventSystem = null;

        public bool InputEnabled { get; private set; } = true;
        public PreferredControls PreferredControl { get; private set; } = PreferredControls.KeyboardMouse;

        public class InputDisabledScope : IDisposable
        {
            public InputDisabledScope()
            {
                GameContext.Instance.InputManager.m_eventSystem.enabled = false;
                GameContext.Instance.InputManager.InputEnabled = false;
            }

            public void Dispose()
            {
                GameContext.Instance.InputManager.m_eventSystem.enabled = true;
                GameContext.Instance.InputManager.InputEnabled = true;
            }
        }

        public void SetPreferredControls(PreferredControls controls)
        {
            // TODO: Save and load this value in Player Prefs
            PreferredControl = controls;
        }
    }
}