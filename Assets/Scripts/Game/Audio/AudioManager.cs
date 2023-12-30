using System;
using System.Collections.Generic;
using Core.Unity.Utils;
using UnityEngine;

namespace Game.Audio
{
    public class AudioManager : SceneSingleton<AudioManager>
    {
        [SerializeField]
        private SFXAudioDatabase m_sfxDatabase = null;

        [SerializeField]
        private AudioPool m_audioPool = null;

        private List<AudioSource> m_activeEffects = new List<AudioSource>();

        public void Play(SFXAudioDatabase.SFXKey sfx)
        {
            SFXAudioDatabase.SFX effect = m_sfxDatabase.GetRandom(sfx);
            if (effect != null)
            {
                Play(effect);
            }
        }

        private void Play(SFXAudioDatabase.SFX effect)
        {
            AudioSource source = m_audioPool.Get();
            source.PlayOneShot(effect.AudioClip);
            m_activeEffects.Add(source);
        }

        private void Update()
        {
            for (int i = m_activeEffects.Count - 1; i >= 0; i--)
            {
                if (!m_activeEffects[i].isPlaying)
                {
                    m_activeEffects.RemoveAt(i);
                }
            }
        }
    }
}