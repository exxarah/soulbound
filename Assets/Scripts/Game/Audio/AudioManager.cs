using System;
using System.Collections.Generic;
using Core.Unity.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Audio
{
    public class AudioManager : SceneSingleton<AudioManager>
    {
        [SerializeField]
        private SFXAudioDatabase m_sfxDatabase = null;

        [SerializeField]
        private MusicAudioDatabase m_musicDatabase = null;

        [SerializeField]
        private AudioPool m_audioPool = null;

        [SerializeField]
        private AudioMixerGroup m_sfxGroup = null;
        
        [SerializeField]
        private AudioMixerGroup m_musicGroup = null;

        private MusicAudioDatabase.MusicKey m_currentMusic = MusicAudioDatabase.MusicKey.None;
        private AudioSource m_currentMusicSource = null;
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
                Instance.m_musicGroup.audioMixer.SetFloat("MusicVolume", s_musicVolume);
            }
        }

        private void OnEnable()
        {
            m_sfxGroup.audioMixer.GetFloat("SFXVolume", out s_sfxVolume);
            SFXVolume = PlayerPrefs.GetFloat("sfx_volume", s_sfxVolume);

            m_musicGroup.audioMixer.GetFloat("MusicVolume", out s_musicVolume);
            MusicVolume = PlayerPrefs.GetFloat("music_volume", s_musicVolume);
        }

        public void Play(SFXAudioDatabase.SFXKey sfx)
        {
            SFXAudioDatabase.SFX effect = m_sfxDatabase.GetRandom(sfx);
            if (effect != null)
            {
                Play(effect);
            }
        }

        public void Play(MusicAudioDatabase.MusicKey music)
        {
            if (music == m_currentMusic) { return; }

            if (m_currentMusicSource != null)
            {
                Fade(m_currentMusicSource, 0.0f, 3.0f, true).Forget();
            }

            if (music != MusicAudioDatabase.MusicKey.None && m_musicDatabase.Entries.TryGetValue(music, out AudioClip musicClip))
            {
                m_currentMusicSource = m_audioPool.Get();
                m_currentMusicSource.loop = true;
                m_currentMusicSource.volume = 0.0f;
                m_currentMusicSource.outputAudioMixerGroup = m_musicGroup;
                m_currentMusicSource.clip = musicClip;
                m_currentMusicSource.Play();
                Fade(m_currentMusicSource, 1.0f, 3.0f).Forget();
            }

            m_currentMusic = music;
        }

        private async UniTask Fade(AudioSource source, float target, float time, bool releaseAtEnd = false)
        {
            float initialVolume = source.volume;
            float timePassed = 0.0f;
            while ((initialVolume < target && source.volume < target) || (initialVolume > target && source.volume > target))
            {
                await UniTask.DelayFrame(1);
                timePassed += Time.deltaTime;
                source.volume = Mathf.Lerp(initialVolume, target, timePassed / time);
            }

            if (releaseAtEnd)
            {
                m_audioPool.Return(source);
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