using Core.Unity.Flow;
using Game.Flow;
using UnityEngine;

namespace Game.Debug
{
    public class DebugPopup : GamePopup
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
    }
}