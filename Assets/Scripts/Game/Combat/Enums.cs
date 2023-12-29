using System;

namespace Game.Combat
{
    public static class Enums
    {
        [Serializable]
        public enum TargetType
        {
            Raycast,
            Cone,
            Sphere,
            Self,
        }

        [Serializable]
        public enum TargetLayer
        {
            Enemies,
            Allies,
            Both,
        }
        
        [Serializable]
        public enum EffectTarget
        {
            Caster,
            Target,
        }
        
        [Serializable]
        public enum EffectTiming
        {
            OnCast,
            OnHit,
        }

        [Serializable]
        public enum CharmType
        {
            OmniCharm,
            HealthCharm,
            AttackCharm,
        }
    }
}