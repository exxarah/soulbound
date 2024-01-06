using UnityEngine;

namespace Game.Combat
{
    public interface IDamageable
    {
        public Transform Transform { get; }
        public int MaxDamagePerHit { get; }
        
        public int DoDamage(DamageParams @params);
        public bool CanBeDamaged(CombatUtils.AttackParams attack);
    }

    public struct DamageParams
    {
        public int DamageAmount;
        public bool ForceKill;
    }
}