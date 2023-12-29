using Core.Unity.Localisation;
using Core.Unity.Utils;
using Game.Data;
using UnityEngine;

namespace Game.UI.Toy
{
    public class CharmCostComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_omniCostParent = null;

        [SerializeField]
        private LocaliseText m_omniCostText = null;
        
        [SerializeField]
        private GameObject m_attackCostParent = null;

        [SerializeField]
        private LocaliseText m_attackCostText = null;
        
        [SerializeField]
        private GameObject m_healthCostParent = null;

        [SerializeField]
        private LocaliseText m_healthCostText = null;
        
        public void Show(CharmCost cost)
        {
            m_omniCostParent.SetActiveSafe(cost.OmniCost > 0);
            m_omniCostText.UpdateParameters(cost.OmniCost.ToString());

            m_attackCostParent.SetActiveSafe(cost.AttackCost > 0);
            m_attackCostText.UpdateParameters(cost.AttackCost.ToString());

            m_healthCostParent.SetActiveSafe(cost.HealthCost > 0);
            m_healthCostText.UpdateParameters(cost.HealthCost.ToString());
        }
    }
}