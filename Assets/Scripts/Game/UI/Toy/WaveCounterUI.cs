using System;
using Core.Unity.Localisation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI.Toy
{
    public class WaveCounterUI : MonoBehaviour
    {
        [FormerlySerializedAs("m_clockText")]
        [SerializeField]
        private LocaliseText m_text = null;

        private void Update()
        {
            // If game has not started yet
            if (!GameContext.Instance.GameState.GameInProgress)
            {
                return;
            }
            
            m_text.UpdateParameters(GameContext.Instance.GameState.WaveCount.ToString(), GameContext.Instance.GameState.GameWaves.ToString());
        }
    }
}