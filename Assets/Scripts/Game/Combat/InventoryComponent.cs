using System;
using Core.Unity.Utils;
using UnityEngine;

namespace Game.Combat
{
    public class InventoryComponent : MonoBehaviour
    {
        [SerializeField]
        private SerializableDictionary<Enums.CharmType, int> m_charmsOwned =
            new SerializableDictionary<Enums.CharmType, int>();

        public bool HasEnoughCharms(int requiredAmount, Enums.CharmType requiredType)
        {
            return false;
        }

        public void IncrementCharms(int amount, Enums.CharmType type)
        {
            if (!m_charmsOwned.TryGetValue(type, out int currentAmount))
            {
                currentAmount = 0;
                m_charmsOwned.Add(type, 0);
            }

            // -1 = unlimited
            if (currentAmount >= 0)
            {
                m_charmsOwned[type] = Math.Max(0, currentAmount + amount);
            }
        }
    }
}