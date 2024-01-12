using Core.Unity.Flow;
using Core.Unity.Localisation;
using Game.Audio;
using UnityEngine;

namespace Game.Flow
{
    public class GameEndedScreen : GameScreen
    {
        public class GameEndedParams : ViewEnterParams
        {
            public bool Won = false;
        }

        [SerializeField]
        private LocaliseText m_headerText = null;
        
        public override void OnViewEnter(ViewEnterParams viewEnterParams = null)
        {
            base.OnViewEnter(viewEnterParams);

            if (viewEnterParams is GameEndedParams gameEndedParams)
            {
                m_headerText.SetKey(gameEndedParams.Won ? "results.win" : "results.lose");
            }
            
            AudioManager.Instance.Play(MusicAudioDatabase.MusicKey.MenuAmbient);
        }

        public void _GoToMainMenu()
        {
            FlowManager.Instance.Trigger("GoToMenu", LoadingScreens.DEFAULT);
        }

        public void _PlayAgain()
        {
            FlowManager.Instance.Trigger("BeginGame", LoadingScreens.CHARACTERS);
        }
    }
}