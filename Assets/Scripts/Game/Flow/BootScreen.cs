using Core.Unity.Flow;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Screen = Core.Unity.Flow.Screen;

namespace Game.Flow
{
    public class BootScreen : Screen
    {
        public override void OnViewEnter(ViewEnterParams viewEnterParams = null)
        {
            base.OnViewEnter(viewEnterParams);

            BootProcess().Forget();
        }

        private async UniTask BootProcess()
        {
            await UniTask.Delay(100);

            await FlowManager.Instance.Trigger("GoToMenu", LoadingScreens.DEFAULT);
        }
    }
}