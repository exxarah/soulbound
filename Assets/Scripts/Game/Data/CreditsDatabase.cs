using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Data
{
    public class CreditsDatabase : ScriptableObject
    {
        [Serializable]
        public class Credit
        {
            [SerializeField]
            private string m_name = null;
            public string Name => m_name;

            [SerializeField]
            private string m_contribution = null;
            public string Contribution => m_contribution;

            [FormerlySerializedAs("m_link")]
            [SerializeField]
            private string m_linkToWork;
            public string LinkToWork => m_linkToWork;
            
            [SerializeField]
            private Sprite m_icon = null;
            public Sprite Icon => m_icon;
        }
        
        [SerializeField]
        private List<Credit> m_credits = new List<Credit>();
        public IReadOnlyList<Credit> Credits => m_credits;
    }
}