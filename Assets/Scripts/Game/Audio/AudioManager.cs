using System;
using System.Collections.Generic;
using Core.Unity.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Audio
{
    public class AudioManager : SceneSingleton<AudioManager>
    {
        [SerializeField]
        private SFXAudioDatabase m_sfxDatabase = null;

        [SerializeField]
        private AudioPool m_audioPool = null;

        [SerializeField]
        private AudioMixerGroup m_sfxGroup = null;
        
        [SerializeField]
        private AudioMixerGroup m_musicGroup = null;

        private List<AudioSource> m_activeEffects = new List<AudioSource>();

        private static float s_sfxVolume;
        public static float SFXVolume
        {
            get => s_sfxVolume;
            set
            {
                s_sfxVolume = value;
                PlayerPrefs.SetFloat("sfx_volume", s_sfxVolume);
                Instance.m_sfxGroup.audioMixer.SetFloat("SFXVolume", s_sfxVolume);
            }
        }
        
        private static float s_musicVolume;
        public static float MusicVolume
        {
            get => s_musicVolume;
            set
            {
                s_musicVolume = value;
                PlayerPrefs.SetFloat("music_volume", s_musicVolume);
                Instance.m_sfxGroup.audioMixer.SetFloat("MusicVolume", s_musicVolume);
            }
        }

        private void OnEnable()
        {
            SFXVolume = PlayerPrefs.GetFloat("sfx_volume", -10.0f);
            MusicVolume = PlayerPrefs.GetFloat("music_volume", -10.0f);
        }

        public void Play(SFXAudioDatabase.SFXKey sfx)
        {
            SFXAudioDatabase.SFX effect = m_sfxDatabase.GetRandom(sfx);
            if (effect != null)
            {
                Play(effect);
            }
        }

        public void PlayAsSFX(AudioClip audioClip)
        {
            AudioSource source = m_audioPool.Get();
            source.outputAudioMixerGroup = m_sfxGroup;
            source.PlayOneShot(audioClip);
            m_activeEffects.Add(source);
        }

        private void Play(SFXAudioDatabase.SFX effect)
        {
            AudioSource source = m_audioPool.Get();
            source.outputAudioMixerGroup = m_sfxGroup;
            source.PlayOneShot(effect.AudioClip);
            m_activeEffects.Add(source);
        }

        private void Update()
        {
            for (int i = m_activeEffects.Count - 1; i >= 0; i--)
            {
                if (!m_activeEffects[i].isPlaying)
                {
                    m_audioPool.Return(m_activeEffects[i]);
                    m_activeEffects.RemoveAt(i);
                }
            }
        }
    }
}