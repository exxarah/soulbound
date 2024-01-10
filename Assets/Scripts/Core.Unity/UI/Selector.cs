using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.Unity.UI
{
    public class Selector : Selectable
    {
        [SerializeField]
        private TMP_Text m_optionText = null;

        private List<string> m_options = new List<string>();
        private int m_currentIndex = 0;

        public int CurrentIndex => m_currentIndex;
        public UnityEvent<int> OnValueChanged;

        public void Populate(List<string> options)
        {
            m_options = options;
            Refresh();
        }

        private void Refresh()
        {
            if (m_currentIndex < m_options.Count)
            {
                m_optionText.text = m_options[m_currentIndex];
            }
        }

        public override Selectable FindSelectableOnLeft()
        {
            if (Application.isPlaying)
            {
                Decrement();   
            }
            return null;
        }

        public override Selectable FindSelectableOnRight()
        {
            if (Application.isPlaying)
            {
                Increment();   
            }
            return null;
        }

        public void Increment()
        {
            m_currentIndex += 1;
            if (m_currentIndex >= m_options.Count)
            {
                m_currentIndex = 0;
            }
            Refresh();
            OnValueChanged?.Invoke(CurrentIndex);
        }

        public void Decrement()
        {
            m_currentIndex -= 1;
            if (m_currentIndex < 0)
            {
                m_currentIndex = m_options.Count - 1;
            }
            Refresh();
            OnValueChanged?.Invoke(CurrentIndex);
        }

        public void SetIndex(int index)
        {
            m_currentIndex = index;
            Refresh();
        }
    }
}