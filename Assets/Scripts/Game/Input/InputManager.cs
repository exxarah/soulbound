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
        public EventSystem EventSystem => m_eventSystem;

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
            string preferredControls = PlayerPrefs.GetString("pref_controls", m_preferredControls.ToString());
            PreferredControl = Enum.Parse<PreferredControls>(preferredControls);
        }

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