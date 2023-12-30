using Core.Unity.Flow;
using Cysharp.Threading.Tasks;
using Game.Audio;

namespace Game.Flow
{
    public class CharacterLoadingScreen : LoadingScreen
    {
        public override async UniTask OnLoadBegin()
        {
            AudioManager.Instance.Play(MusicAudioDatabase.MusicKey.Toy);
            
            // Fake delay so they see the cute loading screen and have time for the music to fade in
            await UniTask.Delay(1000);
        }

        public override UniTask OnLoadEnd()
        {
            return base.OnLoadEnd();
        }
    }
}