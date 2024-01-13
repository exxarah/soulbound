using System;
using System.Collections.Generic;
using Core.Unity.UI;
using Game.Input;
using UnityEngine;

namespace Game.UI.Menus
{
    public class ControlsSelector : MonoBehaviour
    {
        [SerializeField]
        private Selector m_selector = null;

        private List<InputManager.PreferredControls> m_options = new();

        private void Start()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            List<string> strings = new List<string>();
            int selectedIndex = 0;

            m_options.Add(InputManager.PreferredControls.KeyboardMouse);
            strings.Add("settings.controls.kbm");
            if (InputManager.PreferredControls.KeyboardMouse == GameContext.Instance.InputManager.PreferredControl)
            {
                selectedIndex = m_options.Count - 1;
            }
            
            m_options.Add(InputManager.PreferredControls.ControllerXbox);
            strings.Add("settings.controls.xbox");
            if (InputManager.PreferredControls.ControllerXbox == GameContext.Instance.InputManager.PreferredControl)
            {
                selectedIndex = m_options.Count - 1;
            }
            
            m_selector.Populate(strings);
            m_selector.SetIndex(selectedIndex);
        }

        public void OnSelectedChanged(int selectedIndex)
        {
            GameContext.Instance.InputManager.SetPreferredControls(m_options[selectedIndex]);
        }
    }
}