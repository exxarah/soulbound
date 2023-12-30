using Core.Unity.Flow;
using Cysharp.Threading.Tasks;

namespace Game.Flow
{
    public class CharacterLoadingScreen : LoadingScreen
    {
        public override async UniTask OnLoadBegin()
        {
            // Fake delay so they see the cute loading screen
            await UniTask.Delay(1000);
        }

        public override UniTask OnLoadEnd()
        {
            return base.OnLoadEnd();
        }
    }
}