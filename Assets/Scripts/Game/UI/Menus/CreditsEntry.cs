using Core.Unity.Utils;
using Game.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Menus
{
    public class CreditsEntry : Button
    {
        [SerializeField]
        private Image m_icon = null;

        [SerializeField]
        private TMP_Text m_nameText = null;

        [SerializeField]
        private TMP_Text m_contribution = null;

        private CreditsDatabase.Credit m_displayedCredit = null;
        private ScrollRect m_parentScrollRect = null;

        public void Show(CreditsDatabase.Credit credit, ScrollRect scrollRect = null)
        {
            m_displayedCredit = credit;
            m_parentScrollRect = scrollRect;

            m_icon.sprite = credit.Icon;
            m_nameText.text = credit.Name;
            m_contribution.text = credit.Contribution;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            if (m_parentScrollRect != null)
            {
                m_parentScrollRect.ScrollToCenter((RectTransform)this.transform);
            }
        }

        public void _FollowLink()
        {
            Application.OpenURL(m_displayedCredit.LinkToWork);
        }
    }
}