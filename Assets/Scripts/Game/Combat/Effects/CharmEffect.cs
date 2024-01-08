using Core;
using UnityEngine;

namespace Game.Combat.Effects
{
    [CreateAssetMenu(fileName = "CharmEffect", menuName = "Effects/Charm Effect")]
    public class CharmEffect : AAbilityEffect
    {
        public override void ApplyToTarget(Transform targetTransform, GameObject caster)
        {
            // Check if charm is valid
            if (!targetTransform.gameObject.TryGetComponent(out CharmableComponent charmable)) { return; }
            if (!caster.TryGetComponent(out InventoryComponent inventory)) { return; }

            inventory.IncrementCharms(charmable.CharmCount, charmable.CharmType);
            GameContext.Instance.MinionManager.CreateMinion(charmable.CharmType, charmable.CharmCount, targetTransform, caster.transform);
        }
    }
}