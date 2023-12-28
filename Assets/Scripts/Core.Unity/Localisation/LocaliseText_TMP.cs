using TMPro;
using UnityEngine;

namespace Core.Unity.Localisation
{
    [RequireComponent(typeof(TMP_Text))]
    public partial class LocaliseText_TMP : LocaliseText
    {
        [SerializeField]
        [HideInInspector]
        private TMP_Text m_text;

        public override void Refresh()
        {
            m_text.text = GetText();
        }
    }

#if UNITY_EDITOR
    public partial class LocaliseText_TMP
    {
        protected override void OnValidate()
        {
            if (m_text == null) m_text = GetComponent<TMP_Text>();

            base.OnValidate();
        }
    }
#endif
}