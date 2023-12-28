using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Core.Unity.Localisation
{
    public class LocalisationManager : Singleton<LocalisationManager>
    {
        private readonly SystemLanguage m_currentLanguage = SystemLanguage.English;

        private readonly SystemLanguage m_defaultLanguage = SystemLanguage.English;

        private readonly Dictionary<SystemLanguage, LanguageData> m_stringData;

        public Action<SystemLanguage> OnLanguageChanged;

        private LocalisationManager()
        {
            LanguageData[] data;
            if (Application.isPlaying)
            {
                List<LanguageData> languages = new List<LanguageData>();
                foreach (SystemLanguage language in Enum.GetValues(typeof(SystemLanguage)))
                {
                    if (language == SystemLanguage.Unknown) { continue; }

                    string locPath = $"{LanguageToString(language)}.loc";
                    AsyncOperationHandle<IList<IResourceLocation>> location = Addressables.LoadResourceLocationsAsync(locPath, typeof(LanguageData));
                    location.WaitForCompletion();
                    if (location.Status != AsyncOperationStatus.Succeeded || location.Result.Count <= 0) { continue; }

                    AsyncOperationHandle<LanguageData> langData = Addressables.LoadAssetAsync<LanguageData>(locPath);
                    langData.WaitForCompletion();
                    if (langData.Result != null)
                    {
                        languages.Add(langData.Result);
                    }
                }
                data = languages.ToArray();
            }
            else
            {
                string[] guids = AssetDatabase.FindAssets("t:LanguageData");
                data = new LanguageData[guids.Length];
                for (int i = 0; i < guids.Length; i++)
                    data[i] = AssetDatabase.LoadAssetAtPath<LanguageData>(AssetDatabase.GUIDToAssetPath(guids[i]));
            }

            m_stringData = new Dictionary<SystemLanguage, LanguageData>();
            foreach (LanguageData language in data) m_stringData[language.Language] = language;
        }

        public string LocaliseString(string key)
        {
            LanguageData data;
            if (!m_stringData.TryGetValue(m_currentLanguage, out data))
                if (!m_stringData.TryGetValue(m_defaultLanguage, out data))
                    return $"#{key}";

            if (data.TryGet(key, out string translatedString)) return translatedString;

            if (m_stringData[m_defaultLanguage].TryGet(key, out translatedString)) return translatedString;

            return $"#{key}";
        }

        public LanguageData GetLanguageData()
        {
            return GetLanguageData(m_currentLanguage);
        }

        public LanguageData GetLanguageData(SystemLanguage language)
        {
            return m_stringData.GetValueOrDefault(language, null);
        }

        public static string LanguageToString(SystemLanguage language)
        {
            switch (language)
            {
                case SystemLanguage.Afrikaans:
                    return "afr";
                case SystemLanguage.Arabic:
                    return "ara";
                case SystemLanguage.Basque:
                    return "baq";
                case SystemLanguage.Belarusian:
                    return "bel";
                case SystemLanguage.Bulgarian:
                    return "bul";
                case SystemLanguage.Catalan:
                    return "cat";
                case SystemLanguage.Chinese:
                    return "chi";
                case SystemLanguage.Czech:
                    return "cze";
                case SystemLanguage.Danish:
                    return "dan";
                case SystemLanguage.Dutch:
                    return "dut";
                case SystemLanguage.English:
                    return "eng";
                case SystemLanguage.Estonian:
                    return "est";
                case SystemLanguage.Faroese:
                    return "fao";
                case SystemLanguage.Finnish:
                    return "fin";
                case SystemLanguage.French:
                    return "fre";
                case SystemLanguage.German:
                    return "ger";
                case SystemLanguage.Greek:
                    return "gre";
                case SystemLanguage.Hebrew:
                    return "heb";
                case SystemLanguage.Hungarian:
                    return "hun";
                case SystemLanguage.Icelandic:
                    return "ice";
                case SystemLanguage.Indonesian:
                    return "ind";
                case SystemLanguage.Italian:
                    return "ita";
                case SystemLanguage.Japanese:
                    return "jpn";
                case SystemLanguage.Korean:
                    return "kor";
                case SystemLanguage.Latvian:
                    return "lav";
                case SystemLanguage.Lithuanian:
                    return "lit";
                case SystemLanguage.Norwegian:
                    return "nor";
                case SystemLanguage.Polish:
                    return "pol";
                case SystemLanguage.Portuguese:
                    return "por";
                case SystemLanguage.Romanian:
                    return "rum";
                case SystemLanguage.Russian:
                    return "rus";
                case SystemLanguage.SerboCroatian:
                    return "hbs";
                case SystemLanguage.Slovak:
                    return "slo";
                case SystemLanguage.Slovenian:
                    return "slv";
                case SystemLanguage.Spanish:
                    return "spa";
                case SystemLanguage.Swedish:
                    return "swe";
                case SystemLanguage.Thai:
                    return "tha";
                case SystemLanguage.Turkish:
                    return "tur";
                case SystemLanguage.Ukrainian:
                    return "ukr";
                case SystemLanguage.Vietnamese:
                    return "vie";
                case SystemLanguage.ChineseSimplified:
                    return "zhs";
                case SystemLanguage.ChineseTraditional:
                    return "zht";
                case SystemLanguage.Hindi:
                    return "hin";
                case SystemLanguage.Unknown:
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }
}