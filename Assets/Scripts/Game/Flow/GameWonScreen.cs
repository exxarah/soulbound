using Core.Unity.Flow;
using Game.Audio;

namespace Game.Flow
{
    public class GameWonScreen : Screen
    {
        public override void OnViewEnter(ViewEnterParams viewEnterParams = null)
        {
            base.OnViewEnter(viewEnterParams);
            
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