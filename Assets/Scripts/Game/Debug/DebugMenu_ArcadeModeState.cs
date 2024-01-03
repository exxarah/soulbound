using System;
using Core.Unity.Utils;
using TMPro;
using UnityEngine;

namespace Game.Debug
{
    public class DebugMenu_ArcadeModeState : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_noGameInProgress = null;
        
        [SerializeField]
        private TMP_InputField m_currentWave;

        [SerializeField]
        private TMP_InputField m_enemyCount;

        private void OnEnable()
        {
            if (!GameContext.Instance.GameState.GameInProgress)
            {
                m_noGameInProgress.SetActiveSafe(true);
                return;
            }

            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            m_currentWave.text = GameContext.Instance.GameState.WaveCount.ToString();
            m_enemyCount.text = GameContext.Instance.EnemyManager.EnemyCount.ToString();
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        public void _SetCurrentWave(string wave)
        {
            GameContext.Instance.GameState.WaveCount = Convert.ToInt32(wave);
        }  
#endif
    }
}