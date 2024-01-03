using System.Collections.Generic;
using Core.Unity.Flow;
using Core.Unity.Utils;
using Game.Flow;
using UnityEngine;

namespace Game.Debug
{
    public class DebugPopup : GamePopup
    {
        [SerializeField]
        private GameObject m_menuButtons = null;

        [SerializeField]
        private Transform m_menuParent = null;

        private GameObject m_instantiatedDebugMenu = null;
        
        public override void OnViewEnter(ViewEnterParams viewEnterParams = null)
        {
            base.OnViewEnter(viewEnterParams);

            Time.timeScale = 0.0f;
            
            m_menuButtons.SetActiveSafe(true);
        }

        public override void OnViewExit()
        {
            base.OnViewExit();

            Time.timeScale = 1.0f;
        }

        public override void Back()
        {
            if (m_instantiatedDebugMenu != null)
            {
                _OpenDebugButtons();
            }
            else
            {
                FlowManager.Instance.ClosePopup();
            }
        }

        public void _OpenDebugButtons()
        {
            if (m_instantiatedDebugMenu != null)
            {
                Destroy(m_instantiatedDebugMenu);
            }
            m_menuButtons.SetActiveSafe(true);
        }

        public void _OpenDebugMenu(GameObject prefab)
        {
            m_instantiatedDebugMenu = Instantiate(prefab, m_menuParent);
            m_menuButtons.SetActiveSafe(false);
        }
    }
}