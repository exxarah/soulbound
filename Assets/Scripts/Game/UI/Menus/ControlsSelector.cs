using System;
using System.Collections.Generic;
using Core.Unity.Utils;
using Game.Input;
using TMPro;
using UnityEngine;

namespace Game.UI.Menus
{
    public class ControlsSelector : MonoBehaviour
    {
        [SerializeField]
        private Transform m_optionParent = null;
        
        [SerializeField]
        private TMP_Text m_optionTemplate = null;

        private List<TMP_Text> m_options = new List<TMP_Text>();
        private int m_currentIndex = 0;

        private void Awake()
        {
            int index = 0;
            foreach (string option in Enum.GetNames(typeof(InputManager.PreferredControls)))
            {
                TMP_Text text = Instantiate(m_optionTemplate, m_optionParent);
                if (option == GameContext.Instance.InputManager.PreferredControl.ToString())
                {
                    m_currentIndex = index;
                }
                m_options.Add(text);

                index++;
                text.text = option;
            }

            m_optionTemplate.gameObject.SetActiveSafe(false);
            Refresh();
        }

        private void Refresh()
        {
            for (int i = 0; i < m_options.Count; i++)
            {
                m_options[i].gameObject.SetActiveSafe(i == m_currentIndex);
            }
        }

        public void Increment()
        {
            m_currentIndex += 1;
            if (m_currentIndex >= m_options.Count)
            {
                m_currentIndex = 0;
            }
            Refresh();
            GameContext.Instance.InputManager
                       .SetPreferredControls(Enum.Parse<InputManager.PreferredControls>(m_options[m_currentIndex]
                                                .text));
        }

        public void Decrement()
        {
            m_currentIndex -= 1;
            if (m_currentIndex < 0)
            {
                m_currentIndex = m_options.Count - 1;
            }
            Refresh();
            GameContext.Instance.InputManager
                       .SetPreferredControls(Enum.Parse<InputManager.PreferredControls>(m_options[m_currentIndex]
                                                .text));
        }
    }
}