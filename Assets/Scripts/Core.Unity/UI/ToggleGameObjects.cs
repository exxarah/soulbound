using System;
using System.Collections.Generic;
using Core.Unity.Utils;
using UnityEngine;

namespace Core.Unity.UI
{
    public class ToggleGameObjects : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> m_onObjects = new List<GameObject>();

        [SerializeField]
        private List<GameObject> m_offObjects = new List<GameObject>();

        [SerializeField]
        private bool m_currentOn = false;

        private void Start()
        {
            Toggle(m_currentOn);
        }

        public void Toggle(bool isOn)
        {
            m_onObjects.SetActive(isOn);
            m_offObjects.SetActive(!isOn);
            m_currentOn = isOn;
        }

        public void Toggle()
        {
            Toggle(!m_currentOn);
        }
    }
}