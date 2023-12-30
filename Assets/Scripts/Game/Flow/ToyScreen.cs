using Core.Unity.Flow;
using Core.Unity.Utils;
using Cysharp.Threading.Tasks;
using Game.Audio;
using UnityEngine;
using Screen = Core.Unity.Flow.Screen;

namespace Game.Flow
{
    public class ToyScreen : Screen
    {
        [SerializeField]
        private GameObject m_pausePopup = null;

        public override async UniTask OnViewPreEnter(ViewEnterParams viewEnterParams = null)
        {
            await base.OnViewPreEnter(viewEnterParams);
            
            AudioManager.Instance.Play(MusicAudioDatabase.MusicKey.Toy);
        }

        public override void OnViewEnter(ViewEnterParams viewEnterParams = null)
        {
            base.OnViewEnter(viewEnterParams);

            // TODO: Initialise with map-driven values. Spawn map, etc
            GameContext.Instance.GameState.Initialise();
            GameContext.Instance.GameState.OnGameEnded += OnGameEnded;
            
            m_pausePopup.SetActiveSafe(false);
        }

        private void OnGameEnded()
        {
            if (GameContext.Instance.GameState.IsWon)
            {
                FlowManager.Instance.Trigger("GameWon", LoadingScreens.DEFAULT);
            }
            else
            {
                FlowManager.Instance.Trigger("GameLost", LoadingScreens.DEFAULT);
            }
        }

        public void _DoPause()
        {
            Time.timeScale = 0.0f;
            m_pausePopup.SetActiveSafe(true);
        }

        public void _Resume()
        {
            Time.timeScale = 1.0f;
            m_pausePopup.SetActiveSafe(false);
        }

        public void _GoToMenu()
        {
            FlowManager.Instance.Trigger("GoToMenu", LoadingScreens.DEFAULT);
            Time.timeScale = 1.0f;
        }
    }
}