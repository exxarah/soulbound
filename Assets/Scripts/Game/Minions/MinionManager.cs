using System;
using System.Collections.Generic;
using Core.Unity.Utils;
using Game.Combat;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Minions
{
    public class MinionManager : AObjectPool<Minion>
    {
        [SerializeField]
        private Minion m_minionPrefab = null;

        [SerializeField]
        private Transform m_minionRoot = null;

        private Dictionary<Enums.CharmType, List<Minion>> m_activeMinions =
            new Dictionary<Enums.CharmType, List<Minion>>();
        
        private void Start()
        {
            GameContext.Instance.MinionManager = this;
        }

        private void OnDestroy()
        {
            GameContext.Instance.MinionManager = null;
        }

#region Pool Management

        protected override void OnDestroyPoolObject(Minion obj)
        {
            Destroy(obj.gameObject);
        }

        protected override void OnReturnedToPool(Minion obj)
        {
            obj.gameObject.SetActiveSafe(false);
        }

        protected override void OnTakeFromPool(Minion obj)
        {
            obj.gameObject.SetActiveSafe(true);
        }

        protected override Minion CreatePooledItem()
        {
            Minion minion = Instantiate(m_minionPrefab, m_minionRoot);
            return minion;
        }

#endregion

        public void CreateMinion(Enums.CharmType charmType, int amountOfCharms, Transform creator)
        {
            for (int i = 0; i < amountOfCharms; i++)
            {
                Pool.Get(out Minion minion);
                minion.Initialise(charmType, creator);

                if (!m_activeMinions.ContainsKey(charmType))
                {
                    m_activeMinions.Add(charmType, new List<Minion>());
                }
                m_activeMinions[charmType].Add(minion);
            }
        }

        public void ConsumeMinions(params Tuple<Enums.CharmType, int>[] minionsToConsume)
        {
            throw new NotImplementedException();
        }

        public void ConsumeMinions(Enums.CharmType type, int amount)
        {
            throw new NotImplementedException();
        }
    }
}