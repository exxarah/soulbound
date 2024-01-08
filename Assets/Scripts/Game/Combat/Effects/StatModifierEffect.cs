using Game.Entity;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat.Effects
{
    [CreateAssetMenu(fileName = "StatModifierEffect", menuName = "Effects/Stat Modifier Effect")]
    public class StatModifierEffect : AAbilityEffect
    {
        [SerializeField]
        private StatType m_stat;
        
        [FormerlySerializedAs("m_percentageBoost")]
        [SerializeField]
        private float m_percentageChange = 1.5f;

        [SerializeField]
        private float m_effectTime = 0.4f;

        public override void ApplyToTarget(Transform targetTransform, GameObject caster)
        {
            if (targetTransform.TryGetComponent(out Entity.Entity entity))
            {
                entity.Stats.ApplyStatEffect(m_stat, m_effectTime, m_percentageChange);
            }
        }
    }
}