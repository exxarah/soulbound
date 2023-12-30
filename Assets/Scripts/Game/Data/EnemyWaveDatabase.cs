using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public class EnemyWaveDatabase : ScriptableObject
    {
        [Serializable]
        public class WaveDefinition
        {
            [SerializeField]
            private int m_waveNumber;
            public int WaveNumber => m_waveNumber;

            [SerializeField]
            private int m_enemyCount;
            public int EnemyCount => m_enemyCount;
        }

        [SerializeField]
        private List<WaveDefinition> m_entries = new List<WaveDefinition>();
        public List<WaveDefinition> Entries => m_entries;

        public WaveDefinition GetDefinitionForWave(int wave)
        {
            WaveDefinition bestWave = null;
            for (int i = 0; i < m_entries.Count; i++)
            {
                if (m_entries[i].WaveNumber > wave) { continue; }
                if (m_entries[i].WaveNumber == wave) { return m_entries[i]; }
                
                if (bestWave == null || m_entries[i].WaveNumber > bestWave.WaveNumber)
                {
                    bestWave = m_entries[i];
                }
            }

            return bestWave;
        }
    }
}