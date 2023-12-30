using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity
{
    public class EntityStatsComponent : MonoBehaviour
    {
        [SerializeField]
        private float m_baseSpeed = 100.0f;

        private class StatModifier
        {
            public float TimePassed;
            public float TimeToExpire;
            public float ModifierAmount;
        }

        private Dictionary<StatType, List<StatModifier>> m_modifiers = new Dictionary<StatType, List<StatModifier>>();

        public float GetStat(StatType stat)
        {
            float statValue = GetBaseStat(stat);
            if (m_modifiers.TryGetValue(stat, out List<StatModifier> modifiers))
            {
                foreach (StatModifier modifier in modifiers)
                {
                    statValue *= modifier.ModifierAmount;
                }
            }

            return statValue;
        }

        private float GetBaseStat(StatType stat)
        {
            return stat switch
            {
                StatType.Speed => m_baseSpeed,
                _ => throw new ArgumentOutOfRangeException(nameof(stat), stat, null)
            };
        }

        private void Update()
        {
            // Update / Expire existing modifiers
            foreach (StatType statType in m_modifiers.Keys)
            {
                for (int i = m_modifiers[statType].Count - 1; i >= 0; i--)
                {
                    m_modifiers[statType][i].TimePassed += Time.deltaTime;
                    if (m_modifiers[statType][i].TimePassed > m_modifiers[statType][i].TimeToExpire)
                    {
                        m_modifiers[statType].RemoveAt(i);
                    }
                }
            }
        }

        public void ApplyStatEffect(StatType stat, float timeToExpire, float percentageModifier)
        {
            if (!m_modifiers.TryGetValue(stat, out List<StatModifier> modifiers))
            {
                modifiers = new List<StatModifier>();
                m_modifiers.Add(stat, modifiers);
            }
            
            modifiers.Add(new StatModifier(){ModifierAmount = percentageModifier, TimePassed = 0, TimeToExpire = timeToExpire});
        }
    }

    [Serializable]
    public enum StatType
    {
        Speed,
    }
}