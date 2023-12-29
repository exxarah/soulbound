using Game.Data;
using Game.Input;
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
        
        public override void Show(Entity.Entity playerEntity)
        {
            if (!isActiveAndEnabled) { return; }

            string abilityID = playerEntity.AbilitiesComponent.GetAbility(m_actionToDisplay);
            AbilityDatabase.AbilityDefinition abilityDef = Database.Instance.AbilityDatabase.GetAbility(abilityID);

            m_abilityImage.sprite = abilityDef.UIImage;
        }
    }
}