using Game.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menus
{
    public class CreditsEntry : MonoBehaviour
    {
        [SerializeField]
        private Image m_icon = null;

        [SerializeField]
        private TMP_Text m_nameText = null;

        [SerializeField]
        private TMP_Text m_contribution = null;

        private CreditsDatabase.Credit m_displayedCredit = null;

        public void Show(CreditsDatabase.Credit credit)
        {
            m_displayedCredit = credit;

            m_icon.sprite = credit.Icon;
            m_nameText.text = credit.Name;
            m_contribution.text = credit.Contribution;
        }

        public void _FollowLink()
        {
            Application.OpenURL(m_displayedCredit.LinkToWork);
        }
    }
}