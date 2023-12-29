using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Enemy
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [FormerlySerializedAs("m_maximumDistanceAway")]
        [SerializeField]
        private float m_spawnRange = 4.0f;
        
        private int m_spawnPointID;

        private void Start()
        {
            m_spawnPointID = GameContext.Instance.EnemyManager.Register(this);
        }

        private void OnDestroy()
        {
            if (GameContext.Instance.EnemyManager != null)
            {
                GameContext.Instance.EnemyManager.Unregister(m_spawnPointID);
            }
        }

        public Vector3 GetPoint()
        {
            float randomA = Random.Range(0.0f, 1.0f);
            float randomB = Random.Range(0.0f, 1.0f);
            if (randomB < randomA) { Core.Utils.Swap(ref randomA, ref randomB); }

            return transform.position + new Vector3(
                (randomB * m_spawnRange * Mathf.Cos(2 * Mathf.PI * randomA / randomB)), 
                0.0f,
                (randomB * m_spawnRange * Mathf.Sin(2 * Mathf.PI * randomA / randomB))
            );
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color originalColour = UnityEditor.Handles.color;
            
            // Draw range
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, m_spawnRange);

            UnityEditor.Handles.color = originalColour;
        }
#endif
    }
}