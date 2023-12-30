using Core.Unity.Flow;

namespace Game.Flow
{
    public class GameWonScreen : Screen
    {
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