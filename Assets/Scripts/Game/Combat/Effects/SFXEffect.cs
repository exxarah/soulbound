using UnityEngine;

namespace Game.Combat.Effects
{
    [CreateAssetMenu(fileName = "SFXEffect_", menuName = "Effects/SFX Effect")]
    public class SFXEffect : AAbilityEffect
    {
        public override void ApplyToTarget(Transform transform, GameObject caster)
        {
            throw new System.NotImplementedException();
        }
    }
} 