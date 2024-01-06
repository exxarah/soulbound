using System;
using Core.Unity.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Enemy
{
    public class EnemyHealthBarPool : AObjectPool<EnemyHealthBar>
    {
        [SerializeField]
        private EnemyHealthBar m_healthBarTemplate = null;

        [SerializeField]
        private Transform m_parent = null;
        
        protected override void OnDestroyPoolObject(EnemyHealthBar obj)
        {
            Destroy(obj.gameObject);
        }

        protected override void OnReturnedToPool(EnemyHealthBar obj)
        {
            obj.gameObject.SetActiveSafe(false);
        }

        protected override void OnTakeFromPool(EnemyHealthBar obj)
        {
            obj.gameObject.SetActiveSafe(true);
        }

        protected override EnemyHealthBar CreatePooledItem()
        {
            EnemyHealthBar minion = Instantiate(m_healthBarTemplate, m_parent);
            return minion;
        }

        private void OnEnable()
        {
            m_healthBarTemplate.gameObject.SetActiveSafe(false);
        }

        public void OnNewEnemy(Entity.Entity entity)
        {
            EnemyHealthBar healthBar = Pool.Get();
            healthBar.Initialise(entity);
            entity.HealthComponent.OnDead += () => Pool.Release(healthBar);
        }
    }
}