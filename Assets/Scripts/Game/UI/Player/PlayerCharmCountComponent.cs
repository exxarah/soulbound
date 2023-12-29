using Core.Unity.Localisation;
using Game.Combat;
using UnityEngine;

namespace Game.UI.Player
{
    public class PlayerCharmCountComponent : APlayerInformationComponent
    {
        [SerializeField]
        private Enums.CharmType m_charmToDisplay;

        [SerializeField]
        private LocaliseText m_countText = null;
        
        public override void Show(Entity.Entity playerEntity)
        {
            int amount = playerEntity.InventoryComponent.GetCount(m_charmToDisplay);
            m_countText.UpdateParameters(amount.ToString());
        }
    }
}