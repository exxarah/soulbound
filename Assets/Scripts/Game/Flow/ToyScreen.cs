using Core.Unity.Flow;
using Core.Unity.Utils;
using Cysharp.Threading.Tasks;
using Game.Audio;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Flow
{
    public class ToyScreen : GameScreen
    {
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

            GameContext.Instance.InputManager.InputActions.Player.Pause.performed += OnPauseInput;
        }

        public override void OnViewExit()
        {
            base.OnViewExit();

            GameContext.Instance.InputManager.InputActions.Player.Pause.performed -= OnPauseInput;
        }

        private void OnPauseInput(InputAction.CallbackContext obj)
        {
            _DoPause();
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
            if (FlowManager.Instance.CurrentView is PausePopup)
            {
                FlowManager.Instance.ClosePopup();
            }
            else
            {
                FlowManager.Instance.Trigger("Pause");   
            }
        }
    }
}