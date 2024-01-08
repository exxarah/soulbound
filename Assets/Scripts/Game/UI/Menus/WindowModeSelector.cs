using System;
using System.Collections.Generic;
using Core.Unity.UI;
using Core.Unity.Utils;
using Game.Input;
using UnityEngine;

namespace Game.UI.Menus
{
    public class WindowModeSelector : MonoBehaviour
    {
        [SerializeField]
        private Selector m_selector = null;
        
        private List<string> m_options = new List<string>();

        private void Start()
        {
            int selectedIndex = 0;

            m_options.Add(FullScreenMode.FullScreenWindow.ToString());
            if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            {
                selectedIndex = m_options.Count - 1;
            }

#if UNITY_STANDALONE_WIN
            m_options.Add(FullScreenMode.ExclusiveFullScreen.ToString());
            if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
            {
                selectedIndex = m_options.Count - 1;
            }
#endif

#if UNITY_STANDALONE_OSX
            m_options.Add(FullScreenMode.MaximizedWindow.ToString());
            if (Screen.fullScreenMode == FullScreenMode.MaximizedWindow)
            {
                selectedIndex = m_options.Count - 1;
            }
#endif

#if UNITY_STANDALONE
            m_options.Add(FullScreenMode.Windowed.ToString());
            if (Screen.fullScreenMode == FullScreenMode.Windowed)
            {
                selectedIndex = m_options.Count - 1;
            }
#endif

            if (m_options.Count <= 1)
            {
                gameObject.SetActiveSafe(false);
                return;
            }
            m_selector.Populate(m_options);
            m_selector.SetIndex(selectedIndex);
        }

        public void OnSelectedChanged(int selectedIndex)
        {
            if (Enum.TryParse(m_options[selectedIndex], out FullScreenMode value))
            {
                Screen.fullScreenMode = value;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (m_selector == null)
            {
                m_selector = GetComponent<Selector>();
            }
        }
#endif
    }
}