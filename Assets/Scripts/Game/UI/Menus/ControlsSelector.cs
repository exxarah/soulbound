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

        private List<string> m_options = new List<string>();

        private void Start()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            int index = 0;
            int selectedIndex = 0;
            foreach (string option in Enum.GetNames(typeof(InputManager.PreferredControls)))
            {
                if (option == GameContext.Instance.InputManager.PreferredControl.ToString())
                {
                    selectedIndex = index;
                }
                m_options.Add(option);
                index++;
            }
            
            m_selector.Populate(m_options);
            m_selector.SetIndex(selectedIndex);
        }

        public void OnSelectedChanged(int selectedIndex)
        {
            GameContext.Instance.InputManager
                       .SetPreferredControls(Enum.Parse<InputManager.PreferredControls>(m_options[selectedIndex]));
        }
    }
}