using System;
using Core.Unity.Utils;
using Game.Combat;
using UnityEngine;

namespace Game
{
    public class MinionManager : AObjectPool<Entity.Entity>
    {
        [SerializeField]
        private Entity.Entity m_minionPrefab = null;
        
        private void Start()
        {
            GameContext.Instance.MinionManager = this;
        }

        private void OnDestroy()
        {
            GameContext.Instance.MinionManager = null;
        }

#region Pool Management

        protected override void OnDestroyPoolObject(Entity.Entity obj)
        {
            Destroy(obj.gameObject);
        }

        protected override void OnReturnedToPool(Entity.Entity obj)
        {
            obj.gameObject.SetActiveSafe(false);
        }

        protected override void OnTakeFromPool(Entity.Entity obj)
        {
            obj.gameObject.SetActiveSafe(true);
        }

        protected override Entity.Entity CreatePooledItem()
        {
            Entity.Entity minion = Instantiate(m_minionPrefab);
            return minion;
        }

#endregion

        public void CreateMinion(Enums.CharmType charmType, int amountOfCharms)
        {
            
        }
    }
}