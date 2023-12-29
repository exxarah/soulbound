using System.Collections.Generic;
using Core.Unity.Utils;
using Game.Input;
using UnityEngine;

namespace Game.UI.Player
{
    public class PlayerActionListComponent : APlayerInformationComponent
    {
        [SerializeField]
        private SerializableDictionary<FrameInputData.ActionType, GameObject> m_actionDisplays =
            new SerializableDictionary<FrameInputData.ActionType, GameObject>();
        
        public override void Show(Entity.Entity playerEntity)
        {
            foreach (KeyValuePair<FrameInputData.ActionType,GameObject> action in m_actionDisplays)
            {
                action.Value.SetActiveSafe(!string.IsNullOrEmpty(playerEntity.AbilitiesComponent
                                                                            .GetAbility(action.Key)));
            }
        }
    }
}