using System;
using System.Collections.Generic;
using Core.Unity.Utils;
using UnityEngine;

namespace Game.Audio
{
    public class MusicAudioDatabase : ScriptableObject
    {
        [Serializable]
        public enum MusicKey
        {
            None = 0,
            MenuAmbient = 1,
            Toy = 2,
        }

        [SerializeField]
        private SerializableDictionary<MusicKey, AudioClip> m_entries = new();
        internal SerializableDictionary<MusicKey, AudioClip> Entries => m_entries;
    }
}