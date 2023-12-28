using System;
using System.Collections.Generic;
using Core.Unity.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Core.Unity.Localisation
{
    [CreateAssetMenu(fileName = "loc_", menuName = "Localisation/New Language", order = 0)]
    public partial class LanguageData : ScriptableObject
    {
        [SerializeField]
        private SystemLanguage m_language;

        [SerializeField]
        private SerializableDictionary<string, string> m_strings;

        public SystemLanguage Language => m_language;

        public string Get(string key)
        {
            return m_strings[key];
        }

        public bool TryGet(string key, out string translatedString)
        {
            if (m_strings.ContainsKey(key))
            {
                translatedString = Get(key);
                return true;
            }

            translatedString = String.Empty;
            return false;
        }
    }

#if UNITY_EDITOR

    public partial class LanguageData
    {
        private Action<string> onSetIndexCallback;

        public LocalisationSearchProvider GetSearchProvider(Action<string> onSelected)
        {
            return CreateInstance<LocalisationSearchProvider>().Init(this, onSelected);
        }

        public class LocalisationSearchProvider : ScriptableObject, ISearchWindowProvider
        {
            private LanguageData m_languageData;
            private Action<string> m_onSelected;

            public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
            {
                List<SearchTreeEntry> entries = new();

                entries.Add(new SearchTreeGroupEntry(new GUIContent($"Localisation - {m_languageData.m_language}")));

                foreach (KeyValuePair<string, string> value in m_languageData.m_strings)
                {
                    SearchTreeEntry entry = new(new GUIContent($"{value.Key} : {value.Value}"));
                    entry.level = 1;
                    entry.userData = value.Key;

                    entries.Add(entry);
                }

                return entries;
            }

            public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
            {
                m_onSelected?.Invoke(SearchTreeEntry.userData.ToString());
                return true;
            }

            public LocalisationSearchProvider Init(LanguageData languageData, Action<string> onSelected)
            {
                m_languageData = languageData;
                m_onSelected = onSelected;
                return this;
            }
        }
    }

#endif
}