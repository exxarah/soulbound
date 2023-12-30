using System;
using System.Collections.Generic;
using Core.Extensions;
using Core.Unity.Utils;
using UnityEngine;

namespace Game.Audio
{
    public class SFXAudioDatabase : ScriptableObject
    {
        [Serializable]
        public enum SFXKey
        {
            None = 0,
            
            // Game FX 1 - 50
            Hurt = 1,
            
            // UI FX 51 - 70
            ButtonClick = 51,
        }
        
        [Serializable]
        public class SFX
        {
            [SerializeField]
            private AudioClip m_audioClip = null;
            public AudioClip AudioClip => m_audioClip;
        }

        [SerializeField]
        private SerializableDictionary<SFXKey, List<SFX>> m_entries = new SerializableDictionary<SFXKey, List<SFX>>();
        internal SerializableDictionary<SFXKey, List<SFX>> Entries => m_entries;

        public SFX GetRandom(SFXKey sfx)
        {
            return !m_entries.ContainsKey(sfx) ? null : m_entries[sfx].RandomItem();
        }
    }
}