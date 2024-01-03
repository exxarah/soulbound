using System;
using Core.Unity.Flow;
using Game.Debug;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
        public EventSystem EventSystem => m_eventSystem;

        private GameInputActions m_inputActions = null;
        public GameInputActions InputActions => m_inputActions;

        public bool InputEnabled { get; private set; } = true;

        private PreferredControls m_preferredControls = PreferredControls.KeyboardMouse;
        public PreferredControls PreferredControl
        {
            get => m_preferredControls;
            private set
            {
                m_preferredControls = value;
                PlayerPrefs.SetString("pref_controls", m_preferredControls.ToString());
            }
        }

        private void OnEnable()
        {
            m_inputActions = new GameInputActions();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            m_inputActions.Player.OpenDebug.started += ToggleDebug;
#endif
            
            string preferredControls = PlayerPrefs.GetString("pref_controls", m_preferredControls.ToString());
            PreferredControl = Enum.Parse<PreferredControls>(preferredControls);
        }

        private void OnDisable()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            m_inputActions.Player.OpenDebug.started -= ToggleDebug;
#endif
        }
        
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private void ToggleDebug(InputAction.CallbackContext callbackContext)
        {
            if (FlowManager.Instance.CurrentView is DebugPopup)
            {
                FlowManager.Instance.ClosePopup();
                return;
            }

            FlowManager.Instance.Trigger("OpenDebug");
        }
#endif

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
            PreferredControl = controls;
        }
    }
}