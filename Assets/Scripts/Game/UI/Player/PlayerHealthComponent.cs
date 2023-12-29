using Core.Unity.Localisation;
using Core.Unity.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Player
{
    public class PlayerHealthComponent : APlayerInformationComponent
    {
        [SerializeField]
        private LocaliseText m_healthText = null;

        [SerializeField]
        private Image m_fillImage = null;

        [SerializeField]
        private SlicedFilledImage m_slicedFilledImage = null;
        
        public override void Show(Entity.Entity playerEntity)
        {
            if (m_healthText != null)
            {
                m_healthText.UpdateParameters(playerEntity.HealthComponent.CurrentHealth.ToString(), playerEntity.HealthComponent.MaxHealth.ToString());
            }

            if (m_fillImage != null)
            {
                m_fillImage.fillAmount = playerEntity.HealthComponent.HealthPercentage;
            }

            if (m_slicedFilledImage != null)
            {
                m_slicedFilledImage.fillAmount = playerEntity.HealthComponent.HealthPercentage;
            }
        }
    }
}