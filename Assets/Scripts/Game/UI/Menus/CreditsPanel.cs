using Game.Data;
using UnityEngine;

namespace Game.UI.Menus
{
    public class CreditsPanel : MonoBehaviour
    {
        [SerializeField]
        private Transform m_creditsParent = null;

        [SerializeField]
        private CreditsEntry m_creditsPrefab = null;

        private void Start()
        {
            foreach (CreditsDatabase.Credit credit in Database.Instance.CreditsDatabase.Credits)
            {
                CreditsEntry entry = Instantiate(m_creditsPrefab, m_creditsParent);
                entry.Show(credit);
            }
        }
    }
}