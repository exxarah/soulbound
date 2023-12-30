using System;
using System.Collections.Generic;
using System.Linq;
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

            [SerializeField, Range(0, 1)]
            private float m_minimumPercentToSpawn = 0.0f;
            public float MinimumPercentToSpawn => m_minimumPercentToSpawn;

            [SerializeField, Range(0, 1)]
            private float m_maximumPercentToSpawn = 1.0f;
            public float MaximumPercentToSpawn => m_maximumPercentToSpawn;
        }
        
        [SerializeField]
        private List<EnemyDefinition> m_definitions = new List<EnemyDefinition>();
        public IReadOnlyList<EnemyDefinition> Definitions => m_definitions;
        
        private Random m_randomiser = new Random();

        public List<EnemyDefinition> GetEnemiesForWave(int waveCount, int enemyCount)
        {
            Dictionary<string, int> selectedEnemies = new Dictionary<string, int>();
            List<EnemyDefinition> enemySubset = new List<EnemyDefinition>();
            
            // Get our weighted list for this wave
            List<EnemyDefinition> weightedList = new List<EnemyDefinition>();
            for (int i = 0; i < Definitions.Count; i++)
            {
                if (Definitions[i].MinimumWaveToSpawn <= waveCount && Definitions[i].MaximumWaveToSpawn >= waveCount)
                {
                    if (Definitions[i].MinimumPercentToSpawn > 0.0f)
                    {
                        // Add guaranteed / minimum percent enemies
                        int requiredAmount = Math.Max(1, Mathf.RoundToInt(enemyCount * Definitions[i].MinimumPercentToSpawn));
                        for (int j = 0; j < requiredAmount; j++)
                        {
                            enemySubset.Add(Definitions[i]);
                        }

                        if (Definitions[i].MaximumPercentToSpawn < 1.0f)
                        {
                            selectedEnemies.Add(Definitions[i].EnemyID, requiredAmount);
                        }
                    }
                    
                    // Add to list, the same amount of times as weighting. Yes this is wildly inefficient. Yes I'm doing it anyway
                    for (int j = 0; j < Definitions[i].WeightToSpawn; j++)
                    {
                        weightedList.Add(Definitions[i]);
                    }
                }
            }
            
            // Randomly select a subset of enemies to use for this wave
            int preseededEnemies = enemySubset.Count;
            for (int i = 0; i < enemyCount - preseededEnemies; i++)
            {
                if (weightedList.Count <= 0) { break; }

                // Random uses a time-based seed, so we need to use our own cached randomiser, or we'll just get the same enemy over and over
                EnemyDefinition selected = weightedList.RandomItem(m_randomiser);
                
                // Check if this enemy has reached it's maximum percentage, and needs to be removed
                if (selected.MaximumPercentToSpawn < 1.0f)
                {
                    int maximumAmount = Math.Max(1, Math.Min(enemyCount, Mathf.RoundToInt(enemyCount * selected.MaximumPercentToSpawn)));
                    selectedEnemies.TryGetValue(selected.EnemyID, out int currentAmount);

                    if (maximumAmount <= currentAmount)
                    {
                        for (int j = weightedList.Count - 1; j >= 0; j--)
                        {
                            if (weightedList[j].EnemyID == selected.EnemyID)
                            {
                                weightedList.RemoveAt(j);
                            }
                        }

                        // Go back one and try again, since this one wasn't added
                        i--;
                        continue;
                    }
                    else
                    {
                        currentAmount++;
                        selectedEnemies[selected.EnemyID] = currentAmount;
                    }
                }
                
                enemySubset.Add(selected);
            }

            return enemySubset;
        }
    }
}