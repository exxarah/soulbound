using System;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Toy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private Transform m_enemyParent = null;
        
        private int m_activeEnemyCount = 0;

        private void Update()
        {
            if (!GameContext.Instance.GameState.GameInProgress) { return; }

            if (m_activeEnemyCount <= 0)
            {
                SpawnWave();
            }
        }

        private void SpawnWave()
        {
            // TODO: Variable enemy count
            GameContext.Instance.GameState.WaveCount++;
            const int enemyCount = 4;
            List<EnemyDatabase.EnemyDefinition> enemies = Database.Instance.EnemyDatabase.GetEnemiesForWave(GameContext.Instance.GameState.WaveCount, enemyCount);
            for (int i = 0; i < enemies.Count; i++)
            {
                Entity.Entity enemy = Instantiate(enemies[i].EnemyObject, m_enemyParent);
                // TODO: Position it??
                enemy.HealthComponent.OnDead += (() => m_activeEnemyCount--);
            }

            m_activeEnemyCount += enemyCount;
        }
    }
}