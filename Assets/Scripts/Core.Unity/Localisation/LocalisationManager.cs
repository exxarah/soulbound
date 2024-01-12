using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Unity.Localisation
{
    public class LocalisationManager : Singleton<LocalisationManager>
    {
        private readonly SystemLanguage m_currentLanguage = SystemLanguage.English;

        private readonly Dictionary<SystemLanguage, LanguageData> m_stringData;

        public Action<SystemLanguage> OnLanguageChanged;

        public bool DebugTextEnabled = false;

        private LocalisationManager()
        {
            LanguageData[] data = Array.Empty<LanguageData>();
            if (Application.isPlaying)
            {
                data = GameContext.Instance.Localisation;
            }
#if UNITY_EDITOR
            else
            {
                string[] guids = AssetDatabase.FindAssets("t:LanguageData");
                data = new LanguageData[guids.Length];
                for (int i = 0; i < guids.Length; i++)
                    data[i] = AssetDatabase.LoadAssetAtPath<LanguageData>(AssetDatabase.GUIDToAssetPath(guids[i]));
            }      
#endif

            m_stringData = new Dictionary<SystemLanguage, LanguageData>();
            foreach (LanguageData language in data) m_stringData[language.Language] = language;
        }

        public string LocaliseString(string key)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (DebugTextEnabled)
            {
                return "######";
            }      
#endif
            
            LanguageData data;
            if (!m_stringData.TryGetValue(m_currentLanguage, out data))
                return $"#{key}";

            if (data.TryGet(key, out string translatedString)) return translatedString;

            return $"#{key}";
        }

        public LanguageData GetLanguageData()
        {
            return GetLanguageData(m_currentLanguage);
        }

        public LanguageData GetLanguageData(string language)
        {
            language = language.ToLowerInvariant();
            return GetLanguageData(StringToLanguage(language));
        }

        public LanguageData GetLanguageData(SystemLanguage language)
        {
            return m_stringData.GetValueOrDefault(language, null);
        }

        public static SystemLanguage StringToLanguage(string language)
        {
            return language switch
            {
                "afr" => SystemLanguage.Afrikaans,
                "ara" => SystemLanguage.Arabic,
                "baq" => SystemLanguage.Basque,
                "bel" => SystemLanguage.Belarusian,
                "bul" => SystemLanguage.Bulgarian,
                "cat" => SystemLanguage.Catalan,
                "chi" => SystemLanguage.Chinese,
                "cze" => SystemLanguage.Czech,
                "dan" => SystemLanguage.Danish,
                "dut" => SystemLanguage.Dutch,
                "eng" => SystemLanguage.English,
                "est" => SystemLanguage.Estonian,
                "fao" => SystemLanguage.Faroese,
                "fin" => SystemLanguage.Finnish,
                "fre" => SystemLanguage.French,
                "ger" => SystemLanguage.German,
                "gre" => SystemLanguage.Greek,
                "heb" => SystemLanguage.Hebrew,
                "hun" => SystemLanguage.Hungarian,
                "ice" => SystemLanguage.Icelandic,
                "ind" => SystemLanguage.Indonesian,
                "ita" => SystemLanguage.Italian,
                "jpn" => SystemLanguage.Japanese,
                "kor" => SystemLanguage.Korean,
                "lav" => SystemLanguage.Latvian,
                "lit" => SystemLanguage.Lithuanian,
                "nor" => SystemLanguage.Norwegian,
                "pol" => SystemLanguage.Polish,
                "por" => SystemLanguage.Portuguese,
                "rum" => SystemLanguage.Romanian,
                "rus" => SystemLanguage.Russian,
                "hbs" => SystemLanguage.SerboCroatian,
                "slo" => SystemLanguage.Slovak,
                "slv" => SystemLanguage.Slovenian,
                "spa" => SystemLanguage.Spanish,
                "swe" => SystemLanguage.Swedish,
                "tha" => SystemLanguage.Thai,
                "tur" => SystemLanguage.Turkish,
                "ukr" => SystemLanguage.Ukrainian,
                "vie" => SystemLanguage.Vietnamese,
                "zhs" => SystemLanguage.ChineseSimplified,
                "zht" => SystemLanguage.ChineseTraditional,
                "hin" => SystemLanguage.Hindi,
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null),
            };
        }

        public static string LanguageToString(SystemLanguage language)
        {
            return language switch
            {
                SystemLanguage.Afrikaans => "afr",
                SystemLanguage.Arabic => "ara",
                SystemLanguage.Basque => "baq",
                SystemLanguage.Belarusian => "bel",
                SystemLanguage.Bulgarian => "bul",
                SystemLanguage.Catalan => "cat",
                SystemLanguage.Chinese => "chi",
                SystemLanguage.Czech => "cze",
                SystemLanguage.Danish => "dan",
                SystemLanguage.Dutch => "dut",
                SystemLanguage.English => "eng",
                SystemLanguage.Estonian => "est",
                SystemLanguage.Faroese => "fao",
                SystemLanguage.Finnish => "fin",
                SystemLanguage.French => "fre",
                SystemLanguage.German => "ger",
                SystemLanguage.Greek => "gre",
                SystemLanguage.Hebrew => "heb",
                SystemLanguage.Hungarian => "hun",
                SystemLanguage.Icelandic => "ice",
                SystemLanguage.Indonesian => "ind",
                SystemLanguage.Italian => "ita",
                SystemLanguage.Japanese => "jpn",
                SystemLanguage.Korean => "kor",
                SystemLanguage.Latvian => "lav",
                SystemLanguage.Lithuanian => "lit",
                SystemLanguage.Norwegian => "nor",
                SystemLanguage.Polish => "pol",
                SystemLanguage.Portuguese => "por",
                SystemLanguage.Romanian => "rum",
                SystemLanguage.Russian => "rus",
                SystemLanguage.SerboCroatian => "hbs",
                SystemLanguage.Slovak => "slo",
                SystemLanguage.Slovenian => "slv",
                SystemLanguage.Spanish => "spa",
                SystemLanguage.Swedish => "swe",
                SystemLanguage.Thai => "tha",
                SystemLanguage.Turkish => "tur",
                SystemLanguage.Ukrainian => "ukr",
                SystemLanguage.Vietnamese => "vie",
                SystemLanguage.ChineseSimplified => "zhs",
                SystemLanguage.ChineseTraditional => "zht",
                SystemLanguage.Hindi => "hin",
                SystemLanguage.Unknown => throw new ArgumentOutOfRangeException(nameof(language), language, null),
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            };
        }
    }
}