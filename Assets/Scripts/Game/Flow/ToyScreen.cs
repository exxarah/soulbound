using Core.Unity.Flow;
using Screen = Core.Unity.Flow.Screen;

namespace Game.Flow
{
    public class ToyScreen : Screen
    {
        public override void OnViewEnter(ViewEnterParams viewEnterParams = null)
        {
            base.OnViewEnter(viewEnterParams);

            // TODO: Initialise with map-driven values. Spawn map, etc
            GameContext.Instance.GameState.Initialise();
            GameContext.Instance.GameState.OnGameEnded += OnGameEnded;
        }

        private void OnGameEnded()
        {
            if (GameContext.Instance.GameState.IsWon)
            {
                FlowManager.Instance.Trigger("GameWon");
            }
            else
            {
                FlowManager.Instance.Trigger("GameLost");
            }
        }
    }
}