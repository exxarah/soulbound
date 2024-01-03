using System.Collections.Generic;
using Core.Extensions;
using Game.Data;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private Transform m_enemyParent = null;

        private List<EnemySpawnPoint> m_availableSpawnPoints = new List<EnemySpawnPoint>();

        private int m_activeEnemyCount = 0;
        public int EnemyCount => m_activeEnemyCount;

        private void Awake()
        {
            GameContext.Instance.EnemyManager = this;
        }

        private void OnDestroy()
        {
            GameContext.Instance.EnemyManager = null;
        }

        private void Update()
        {
            if (!GameContext.Instance.GameState.GameInProgress) { return; }

            if (m_activeEnemyCount <= 0 || (GameContext.Instance.GameState.WaveCount < GameContext.Instance.GameState.GameWaves && m_activeEnemyCount <= 1))
            {
                SpawnWave();
            }
        }

        private void SpawnWave()
        {
            GameContext.Instance.GameState.WaveCount++;
            EnemyWaveDatabase.WaveDefinition waveDef = GameContext.Instance.Database.WaveDatabase.GetDefinitionForWave(GameContext.Instance.GameState.WaveCount);

            EnemySpawnPoint waveSpawnPoint = m_availableSpawnPoints.RandomItem();
            List<EnemyDatabase.EnemyDefinition> enemies = GameContext.Instance.Database.EnemyDatabase.GetEnemiesForWave(GameContext.Instance.GameState.WaveCount, waveDef.EnemyCount);
            for (int i = 0; i < enemies.Count; i++)
            {
                Entity.Entity enemy = Instantiate(enemies[i].EnemyObject, m_enemyParent);
                enemy.Rigidbody.transform.position = waveSpawnPoint.GetPoint();
                enemy.HealthComponent.OnDead += (() => m_activeEnemyCount--);
            }

            m_activeEnemyCount += enemies.Count;
        }

        public int Register(EnemySpawnPoint spawnPoint)
        {
            for (int i = 0; i < m_availableSpawnPoints.Count; i++)
            {
                if (m_availableSpawnPoints[i] == null)
                {
                    m_availableSpawnPoints[i] = spawnPoint;
                    return i;
                }
            }
            m_availableSpawnPoints.Add(spawnPoint);
            return m_availableSpawnPoints.Count - 1;
        }

        public void Unregister(int spawnPointID)
        {
            m_availableSpawnPoints[spawnPointID] = null;
            
            // Clean up spawn points
            for (int i = m_availableSpawnPoints.Count - 1; i >= 0; i--)
            {
                if (m_availableSpawnPoints[i] == null)
                {
                    m_availableSpawnPoints.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }
    }
}