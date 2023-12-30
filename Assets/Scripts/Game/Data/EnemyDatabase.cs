using System;
using System.Collections.Generic;
using Core.Extensions;
using UnityEngine;
using Random = System.Random;

namespace Game.Data
{
    public class EnemyDatabase : ScriptableObject
    {
        [Serializable]
        public class EnemyDefinition
        {
            [SerializeField]
            private string m_enemyID;
            public string EnemyID => m_enemyID;

            [SerializeField]
            private Entity.Entity m_enemyObject = null;
            public Entity.Entity EnemyObject => m_enemyObject;

            [SerializeField]
            private int m_minimumWaveToSpawn = 0;
            public int MinimumWaveToSpawn => m_minimumWaveToSpawn;

            [SerializeField]
            private int m_maximumWaveToSpawn = int.MaxValue;
            public int MaximumWaveToSpawn => m_maximumWaveToSpawn;

            [SerializeField, Min(1)]
            private int m_weightToSpawn = 5;
            public int WeightToSpawn => m_weightToSpawn;
        }
        
        [SerializeField]
        private List<EnemyDefinition> m_definitions = new List<EnemyDefinition>();
        public IReadOnlyList<EnemyDefinition> Definitions => m_definitions;

        private Dictionary<int, List<EnemyDefinition>> m_weightedListForWave = new();
        private Random m_randomiser = new Random();

        public List<EnemyDefinition> GetEnemiesForWave(int waveCount, int enemyCount)
        {
            // Get our weighted list for this wave
            if (!m_weightedListForWave.TryGetValue(waveCount, out List<EnemyDefinition> weightedList))
            {
                // Make our own weighted list, and store it for future use
                weightedList = new List<EnemyDefinition>();
                for (int i = 0; i < Definitions.Count; i++)
                {
                    if (Definitions[i].MinimumWaveToSpawn <= waveCount && Definitions[i].MaximumWaveToSpawn >= waveCount)
                    {
                        // Add to list, the same amount of times as weighting. Yes this is wildly inefficient. Yes I'm doing it anyway
                        for (int j = 0; j < Definitions[i].WeightToSpawn; j++)
                        {
                            weightedList.Add(Definitions[i]);
                        }
                    }
                }

                m_weightedListForWave[waveCount] = weightedList;
            }
            
            // Randomly select a subset of enemies to use for this wave
            List<EnemyDefinition> enemySubset = new List<EnemyDefinition>();
            for (int i = 0; i < enemyCount; i++)
            {
                // Random uses a time-based seed, so we need to use our own cached randomiser, or we'll just get the same enemy over and over
                enemySubset.Add(weightedList.RandomItem(m_randomiser));
            }

            return enemySubset;
        }
    }
}