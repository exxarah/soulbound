using System;
using Core.Unity.Utils;
using Game.Data;
using UnityEngine;

namespace Game.Combat
{
    public class InventoryComponent : MonoBehaviour
    {
        [SerializeField]
        private SerializableDictionary<Enums.CharmType, int> m_charmsOwned =
            new SerializableDictionary<Enums.CharmType, int>();
        
        public bool CanAfford(CharmCost cost)
        {
            int omnisUsed = 0;
            // TODO: Do it all at once so we can keep track of omnis required as other types
            if (cost.AttackCost > 0)
            {
                if (!CanAfford(Enums.CharmType.AttackCharm, cost.AttackCost, out int omnis))
                {
                    return false;
                }

                omnisUsed += omnis;
            }
            
            if (cost.HealthCost > 0)
            {
                if (!CanAfford(Enums.CharmType.HealthCharm, cost.HealthCost, out int omnis))
                {
                    return false;
                }

                omnisUsed += omnis;
            }
            
            // Do omnis last, we'll check the cost of them combined with the cost of any omnis used for other types
            if (cost.OmniCost > 0 || omnisUsed > 0)
            {
                if (!CanAfford(Enums.CharmType.OmniCharm, cost.OmniCost + omnisUsed, out int omnis))
                {
                    return false;
                }
            }

            return true;
        }

        public bool CanAfford(Enums.CharmType charmType, int requiredAmount, out int omnisUsed)
        {
            omnisUsed = 0;

            m_charmsOwned.TryGetValue(charmType, out int typedAmount);
            // -1 == unlimited
            if (typedAmount == -1) { return true; }
            if (typedAmount >= requiredAmount) { return true; }
            
            // If we're checking that we can afford omnis, we can't use omnis to supplement
            if (charmType == Enums.CharmType.OmniCharm) { return false; }
            
            // Need to use some omnis
            m_charmsOwned.TryGetValue(Enums.CharmType.OmniCharm, out int omniAmount);
            // Not enough omnis
            if (typedAmount >= 0 && typedAmount + omniAmount < requiredAmount)
            {
                return false;
            }
            
            // We have enough omnis to supplement this, calculate how many we need to spend
            omnisUsed = requiredAmount - typedAmount;
            return true;
        }

        public void IncrementCharms(int amount, Enums.CharmType type)
        {
            if (amount == 0) { return; }

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

        public void Spend(CharmCost charmCost)
        {
            int omnisUsed = 0;
            if (charmCost.AttackCost > 0 && CanAfford(Enums.CharmType.AttackCharm, charmCost.AttackCost, out omnisUsed))
            {
                IncrementCharms(-charmCost.AttackCost - omnisUsed, Enums.CharmType.AttackCharm);
                IncrementCharms(-omnisUsed, Enums.CharmType.OmniCharm);

                // Kill the minions
                GameContext.Instance.MinionManager.ConsumeMinions(Enums.CharmType.AttackCharm, charmCost.AttackCost - omnisUsed);
                GameContext.Instance.MinionManager.ConsumeMinions(Enums.CharmType.OmniCharm, omnisUsed);
            }
            if (charmCost.HealthCost > 0 && CanAfford(Enums.CharmType.HealthCharm, charmCost.HealthCost, out omnisUsed))
            {
                IncrementCharms(-charmCost.HealthCost - omnisUsed, Enums.CharmType.HealthCharm);
                IncrementCharms(-omnisUsed, Enums.CharmType.OmniCharm);
                
                // Kill the minions
                GameContext.Instance.MinionManager.ConsumeMinions(Enums.CharmType.HealthCharm, charmCost.HealthCost - omnisUsed);
                GameContext.Instance.MinionManager.ConsumeMinions(Enums.CharmType.OmniCharm, omnisUsed);
            }
            if (charmCost.OmniCost > 0 && CanAfford(Enums.CharmType.OmniCharm, charmCost.OmniCost, out omnisUsed))
            {
                IncrementCharms(-charmCost.OmniCost, Enums.CharmType.OmniCharm);
                
                // Kill the minions
                GameContext.Instance.MinionManager.ConsumeMinions(Enums.CharmType.OmniCharm, charmCost.OmniCost);
            }
        }

        public int GetCount(Enums.CharmType charmToDisplay)
        {
            if (m_charmsOwned.TryGetValue(charmToDisplay, out int amount))
            {
                return amount;
            }

            return 0;
        }
    }
}