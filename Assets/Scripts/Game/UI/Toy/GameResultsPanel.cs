using System;
using Core.Unity.Localisation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI.Toy
{
    public class GameResultsPanel : MonoBehaviour
    {
        [SerializeField]
        private LocaliseText m_finalTime = null;

        [SerializeField]
        private LocaliseText m_damageTaken = null;

        [SerializeField]
        private LocaliseText m_damageGiven = null;

        [SerializeField]
        private LocaliseText m_enemiesCharmed = null;
        
        [FormerlySerializedAs("m_wavesCompleted")]
        [SerializeField]
        private LocaliseText m_minionsSpent = null;

        [SerializeField]
        private LocaliseText m_finalScore = null;

        private void OnEnable()
        {
            m_finalTime.UpdateParameters(TimeSpan.FromSeconds(GameContext.Instance.GameState.SecondsPassed).ToString("mm\\:ss"));
            m_minionsSpent.UpdateParameters(GameContext.Instance.GameState.MinionsSpent.ToString());
            m_enemiesCharmed.UpdateParameters(GameContext.Instance.GameState.MinionsEarned.ToString());
            m_damageTaken.UpdateParameters(GameContext.Instance.GameState.PlayerDamageTaken.ToString());
            m_damageGiven.UpdateParameters(GameContext.Instance.GameState.PlayerDamageGiven.ToString());
            
            m_finalScore.UpdateParameters(CalculateFinalScore().ToString());
        }

        private int CalculateFinalScore()
        {
            float score = 0;

            score -= GameContext.Instance.GameState.SecondsPassed / 10.0f;
            score += GameContext.Instance.GameState.MinionsSpent * 3.0f;
            score += GameContext.Instance.GameState.MinionsEarned * 5.0f;
            // Minions left unconsumed
            score += (GameContext.Instance.GameState.MinionsEarned - GameContext.Instance.GameState.MinionsSpent) * 15.0f;
            score += GameContext.Instance.GameState.PlayerDamageGiven * 2.0f;
            score -= GameContext.Instance.GameState.PlayerDamageTaken * 2.0f;

            return Mathf.RoundToInt(score);
        }
    }
}