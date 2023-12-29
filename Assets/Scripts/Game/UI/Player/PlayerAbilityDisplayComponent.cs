using System;
using Core.Unity.Utils;
using Game.Data;
using Game.Input;
using Game.UI.Toy;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Player
{
    public class PlayerAbilityDisplayComponent : APlayerInformationComponent, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private FrameInputData.ActionType m_actionToDisplay;

        [SerializeField]
        private Animator m_animator = null;

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

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_animator.SetBool("pointerOver", true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_animator.SetBool("pointerOver", false);
        }

        public void _SimulateInput()
        {
            FrameInputData input = new();
            input.SetAction(m_actionToDisplay, true);
            m_playerEntity.ApplyInput(input);
        }
    }
}