﻿using Core.Unity.Flow;
using Cysharp.Threading.Tasks;

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

            FlowManager.Instance.Trigger("GoToMenu", LoadingScreens.DEFAULT);
        }
    }
}