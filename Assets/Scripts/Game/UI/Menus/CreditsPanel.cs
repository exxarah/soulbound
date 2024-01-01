using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Unity.Utils;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menus
{
    public class CreditsPanel : MonoBehaviour
    {
        [SerializeField]
        private Transform m_creditsParent = null;

        [SerializeField]
        private CreditsEntry m_creditsPrefab = null;

        [SerializeField]
        private ScrollRect m_scrollRect = null;

        [SerializeField, ReadOnly]
        private List<CreditsEntry> m_instantiatedCredits = new List<CreditsEntry>();

        private void Awake()
        {
            m_instantiatedCredits.Clear();
            foreach (CreditsDatabase.Credit credit in GameContext.Instance.Database.CreditsDatabase.Credits)
            {
                CreditsEntry entry = Instantiate(m_creditsPrefab, m_creditsParent);
                entry.Show(credit, m_scrollRect);
                m_instantiatedCredits.Add(entry);
            }
        }

        private void OnEnable()
        {
            if (m_instantiatedCredits.Count == 0) { return; }
            CreditsEntry firstEntry = m_instantiatedCredits.First();
            if (firstEntry == null) { return; }

            StartCoroutine(SelectEntry(firstEntry));
        }

        private IEnumerator SelectEntry(CreditsEntry entry)
        {
            yield return null;

            GameContext.Instance.InputManager.EventSystem.SetSelectedGameObject(entry.gameObject);
            m_scrollRect.ScrollToCenter((RectTransform)entry.transform);
        }
    }
}