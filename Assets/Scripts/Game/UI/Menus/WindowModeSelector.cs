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

        private List<FullScreenMode> m_optionModes = new List<FullScreenMode>();

        private void Start()
        {
            List<string> strings = new List<string>();
            int selectedIndex = 0;

            m_optionModes.Add(FullScreenMode.FullScreenWindow);
            strings.Add("settings.window.borderless");
            if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            {
                selectedIndex = m_optionModes.Count - 1;
            }

#if UNITY_STANDALONE_WIN
            m_optionModes.Add(FullScreenMode.ExclusiveFullScreen);
            strings.Add("settings.window.fullscreen");
            if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
            {
                selectedIndex = m_optionModes.Count - 1;
            }
#endif

#if UNITY_STANDALONE_OSX
            m_optionModes.Add(FullScreenMode.MaximizedWindow);
            m_options.Add("settings.window.borderless");
            if (Screen.fullScreenMode == FullScreenMode.MaximizedWindow)
            {
                selectedIndex = m_optionModes.Count - 1;
            }
#endif

#if UNITY_STANDALONE
            m_optionModes.Add(FullScreenMode.Windowed);
            strings.Add("settings.window.windowed");
            if (Screen.fullScreenMode == FullScreenMode.Windowed)
            {
                selectedIndex = m_optionModes.Count - 1;
            }
#endif

            if (strings.Count <= 1)
            {
                gameObject.SetActiveSafe(false);
                return;
            }
            m_selector.Populate(strings);
            m_selector.SetIndex(selectedIndex);
        }

        public void OnSelectedChanged(int selectedIndex)
        {
            Resolution currentResolution = Screen.currentResolution;
            switch (m_optionModes[selectedIndex])
            {
                case FullScreenMode.Windowed:
                {
                    DisplayInfo windowInfo = Screen.mainWindowDisplayInfo;
                    const int windowWidth = 1280;
                    const int windowHeight = 720;
                    Screen.SetResolution(windowWidth, windowHeight, FullScreenMode.Windowed);
                    Screen.MoveMainWindowTo(in windowInfo, new Vector2Int(windowInfo.width / 2 - windowWidth / 2, windowInfo.height / 2 - windowHeight / 2));
                    break;
                }
                case FullScreenMode.ExclusiveFullScreen:
                {
                    Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.ExclusiveFullScreen);
                    break;
                }
                case FullScreenMode.FullScreenWindow:
                {
                    Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.FullScreenWindow);
                    break;
                }
                case FullScreenMode.MaximizedWindow:
                {
                    Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.MaximizedWindow);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
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