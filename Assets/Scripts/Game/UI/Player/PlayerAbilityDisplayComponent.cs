using System;
using Core.Unity.Utils;
using Game.Data;
using Game.Input;
using Game.UI.Toy;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Player
{
    public class PlayerAbilityDisplayComponent : APlayerInformationComponent
    {
        [SerializeField]
        private FrameInputData.ActionType m_actionToDisplay;

        [SerializeField]
        private Image m_abilityImage = null;

        [SerializeField]
        private SlicedFilledImage m_cooldownCover = null;

        [SerializeField]
        private CharmCostComponent m_costComponent = null;

        private Entity.Entity m_playerEntity = null;
        private AbilityDatabase.AbilityDefinition m_abilityDefinition = null;
        
        public override void Show(Entity.Entity playerEntity)
        {
            m_playerEntity = playerEntity;
        }

        private void Update()
        {
            if (!isActiveAndEnabled || m_playerEntity == null) { return; }

            string abilityID = m_playerEntity.AbilitiesComponent.GetAbility(m_actionToDisplay);
            if (m_abilityDefinition == null || abilityID != m_abilityDefinition.AbilityID)
            {
                m_abilityDefinition = Database.Instance.AbilityDatabase.GetAbility(abilityID);
                if (m_abilityDefinition == null) { return; }

                m_abilityImage.sprite = m_abilityDefinition.UIImage;
                
                m_costComponent.Show(m_abilityDefinition.CharmCost);
            }

            m_cooldownCover.fillAmount = m_playerEntity.GetCooldown(m_abilityDefinition);
        }
    }
}