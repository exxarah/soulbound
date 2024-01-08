using Game.Audio;
using UnityEngine;

namespace Game.Combat.Effects
{
    [CreateAssetMenu(fileName = "SFXEffect_", menuName = "Effects/SFX Effect")]
    public class SFXEffect : AAbilityEffect
    {
        [SerializeField]
        private SFXAudioDatabase.SFXKey m_sfxKey = SFXAudioDatabase.SFXKey.None;

        [SerializeField]
        private AudioClip m_audioClip = null;
        
        public override void ApplyToTarget(Transform targetTransform, GameObject caster)
        {
            if (m_sfxKey != SFXAudioDatabase.SFXKey.None)
            {
                AudioManager.Instance.Play(m_sfxKey);
                return;
            }

            if (m_audioClip != null)
            {
                AudioManager.Instance.PlayAsSFX(m_audioClip);
            }
        }
    }
} 