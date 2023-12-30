using UnityEngine;

namespace Game.Combat
{
    public class CharmableComponent : MonoBehaviour
    {
        [SerializeField]
        private Enums.CharmType m_charmType;
        public Enums.CharmType CharmType => m_charmType;

        [SerializeField]
        private int m_charmCount = 1;
        public int CharmCount => m_charmCount;
    }
}