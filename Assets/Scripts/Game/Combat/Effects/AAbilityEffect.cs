using UnityEngine;

namespace Game.Combat.Effects
{
    public abstract class AAbilityEffect : ScriptableObject
    {
        [SerializeField]
        private Enums.EffectTiming m_timing = Enums.EffectTiming.OnHit;
        public Enums.EffectTiming Timing => m_timing;
        
        [SerializeField]
        private Enums.EffectTarget m_target = Enums.EffectTarget.Target;
        public Enums.EffectTarget Target => m_target;
        
        public abstract void ApplyToTarget(Transform target, GameObject caster);
    }
}