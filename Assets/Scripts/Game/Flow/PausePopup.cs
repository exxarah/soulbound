using Core.Unity.Flow;
using UnityEngine;

namespace Game.Flow
{
    public class PausePopup : Popup
    {
        public override void OnViewEnter(ViewEnterParams viewEnterParams = null)
        {
            base.OnViewEnter(viewEnterParams);

            Time.timeScale = 0.0f;
        }

        public override void OnViewExit()
        {
            base.OnViewExit();

            Time.timeScale = 1.0f;
        }

        public void _GoToMenu()
        {
            FlowManager.Instance.Trigger("GoToMenu", LoadingScreens.DEFAULT);
        }
    }
}