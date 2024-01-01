using Core.Unity.Flow;
using UnityEngine;
using UnityEngine.UI;
using Screen = Core.Unity.Flow.Screen;

namespace Game.Flow
{
    public class GameScreen : Screen
    {
        [SerializeField]
        private Selectable m_defaultSelected = null;

        public override void OnViewEnter(ViewEnterParams viewEnterParams = null)
        {
            base.OnViewEnter(viewEnterParams);

            if (m_defaultSelected != null)
            {
                GameContext.Instance.InputManager.EventSystem.SetSelectedGameObject(m_defaultSelected.gameObject);
            }
        }
    }
}