using Cysharp.Threading.Tasks;
using Game.Audio;
using UnityEngine;

namespace Game.Combat.Effects
{
    [CreateAssetMenu(fileName = "SFXSustainedEffect_", menuName = "Effects/SFX Sustained Effect")]
    public class SFXSustainedEffect : AAbilityEffect
    {
        [SerializeField]
        private SFXAudioDatabase.SFXKey m_sfxKey = SFXAudioDatabase.SFXKey.None;

        [SerializeField]
        private int m_amountToPlay = 3;

        [SerializeField]
        private float m_minTimeBetweenTriggers = 0.2f;

        [SerializeField]
        private float m_maxTimeBetweenTriggers = 0.5f;
        
        public override void ApplyToTarget(Transform target, GameObject caster)
        {
            TriggerSFX().Forget();
        }

        private async UniTask TriggerSFX()
        {
            for (int i = 0; i < m_amountToPlay; i++)
            {
                AudioManager.Instance.Play(m_sfxKey);
                await UniTask.Delay(Mathf.RoundToInt(Random.Range(m_minTimeBetweenTriggers, m_maxTimeBetweenTriggers) * 1000));
            }
        }
    }
}