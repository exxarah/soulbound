using System;
using UnityEngine;

namespace Core.Unity.Localisation
{
    public abstract partial class LocaliseText : MonoBehaviour
    {
        [SerializeField]
        private string m_localisationKey;

        private string[] m_parameters;

        private void OnEnable()
        {
            Refresh();

            LocalisationManager.Instance.OnLanguageChanged += OnLanguageChanged;
        }

        private void OnDisable()
        {
            LocalisationManager.Instance.OnLanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(SystemLanguage newLanguage)
        {
            Refresh();
        }

        public void SetKey(string newKey)
        {
            m_localisationKey = newKey;

            Refresh();
        }

        public void UpdateParameters(params string[] @params)
        {
            m_parameters = @params;

            Refresh();
        }

        private static string FormatString(string key, params string[] @params)
        {
            string loc = LocalisationManager.Instance.LocaliseString(key);
            return (@params == null || @params.Length == 0)? loc : String.Format(loc, @params);
        }

        protected string GetText()
        {
            return FormatString(m_localisationKey, m_parameters);
        }

        public abstract void Refresh();
    }

#if UNITY_EDITOR
    public partial class LocaliseText
    {
        protected virtual void OnValidate()
        {
            Refresh();
        }
    }
#endif
}