using System;
using Core.Unity.Localisation;
using UnityEngine;

namespace Game.UI.Toy
{
    public class GameClockUI : MonoBehaviour
    {
        [SerializeField]
        private LocaliseText m_clockText = null;

        private void Update()
        {
            // If game has not started yet
            if (!GameContext.Instance.GameState.GameInProgress && GameContext.Instance.GameState.SecondsPassed <= 0.0f)
            {
                return;
            }
            
            m_clockText.UpdateParameters(TimeSpan.FromSeconds(GameContext.Instance.GameState.SecondsPassed).ToString("mm\\:ss"));
        }
    }
}