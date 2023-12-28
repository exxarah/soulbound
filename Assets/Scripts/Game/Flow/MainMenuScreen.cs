using Core.Unity.Flow;
using UnityEditor;
using UnityEngine;
using Screen = Core.Unity.Flow.Screen;

namespace Game.Flow
{
    public class MainMenuScreen : Screen
    {
        public void _PlayGame()
        {
            FlowManager.Instance.Trigger("BeginGame");
        }

        public void _QuitGame()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                EditorApplication.ExitPlaymode();
                return;
            }
#else
            Application.Quit();
#endif
        }

        public void _OpenSettings()
        {
            
        }
    }
}