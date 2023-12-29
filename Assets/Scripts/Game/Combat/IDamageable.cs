using UnityEngine;

namespace Game.Combat
{
    public interface IDamageable
    {
        public Transform Transform { get; }
        
        public void DoDamage(DamageParams @params);
        public bool CanBeDamaged();
    }

    public struct DamageParams
    {
        public int DamageAmount;
        public bool ForceKill;
    }
}