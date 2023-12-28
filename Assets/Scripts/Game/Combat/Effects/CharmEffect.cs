using UnityEngine;

namespace Game.Combat.Effects
{
    [CreateAssetMenu(fileName = "CharmEffect", menuName = "Effects/Charm Effect")]
    public class CharmEffect : AAbilityEffect
    {
        public override void ApplyToTarget(Transform transform)
        {
            // Check if charm is valid
            if (!transform.gameObject.TryGetComponent(out CharmableComponent charmable)) { return; }
        }
    }
}