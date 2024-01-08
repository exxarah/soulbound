using Core.Unity.Utils;
using Game.Toy;
using UnityEngine;

namespace Game.Combat.Effects
{
    [CreateAssetMenu(fileName = "VFXEffect_", menuName = "Effects/VFX Effect")]
    public class VFXEffect : AAbilityEffect
    {
        [SerializeField]
        private GameObject m_effectPrefab = null;

        [SerializeField]
        private float m_secondsTillDeath = 1.0f;

        public override void ApplyToTarget(Transform targetTransform, GameObject caster)
        {
            GameObject effect = Instantiate(m_effectPrefab, targetTransform);
            effect.layer = LayerMask.NameToLayer(Layers.EFFECTS);
            effect.transform.position = targetTransform.position;
            
            // Add effect killer component
            TimedDestroyComponent component = effect.AddComponent<TimedDestroyComponent>();
            component.Initialise(m_secondsTillDeath);
        }
    }
}