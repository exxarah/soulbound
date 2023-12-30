using System;
using Game.Input;
using UnityEngine;

namespace Game.Combat
{
    public class EquippedAbilitiesComponent : MonoBehaviour
    {
        [SerializeField]
        private string m_shieldAbility = "";

        [SerializeField]
        private string m_basicAbility = "";

        [SerializeField]
        private string m_abilityOne = "";

        [SerializeField]
        private string m_abilityTwo = "";

        [SerializeField]
        private string m_abilityThree = "";

        [SerializeField]
        private string m_abilityFour = "";

        [SerializeField]
        private string m_charmAbility = "";

        public string GetAbility(FrameInputData.ActionType action)
        {
            return action switch
            {
                FrameInputData.ActionType.BasicMitigation => m_shieldAbility,
                FrameInputData.ActionType.BasicAttack => m_basicAbility,
                FrameInputData.ActionType.AbilityOne => m_abilityOne,
                FrameInputData.ActionType.AbilityTwo => m_abilityTwo,
                FrameInputData.ActionType.AbilityThree => m_abilityThree,
                FrameInputData.ActionType.AbilityFour => m_abilityFour,
                FrameInputData.ActionType.CharmAbility => m_charmAbility,
                _ => throw new ArgumentOutOfRangeException(nameof(action), action, null),
            };
        }
    }
}