using System;
using UnityEngine;

namespace Game.Input
{
    public struct FrameInputData
    {
        public Vector2 MovementDirection;

        public bool Shield;
        public bool BasicAttack;

        public bool AbilityOne;
        public bool AbilityTwo;
        public bool AbilityThree;
        public bool AbilityFour;

        public bool CharmAbility;

        public enum ActionType
        {
            Shield,
            BasicAttack,

            AbilityOne,
            AbilityTwo,
            AbilityThree,
            AbilityFour,

            CharmAbility,
        }

        public void SetAction(ActionType action, bool enabled)
        {
            switch (action)
            {
                case ActionType.Shield:
                    Shield = enabled;
                    break;
                case ActionType.BasicAttack:
                    BasicAttack = enabled;
                    break;
                case ActionType.AbilityOne:
                    AbilityOne = enabled;
                    break;
                case ActionType.AbilityTwo:
                    AbilityTwo = enabled;
                    break;
                case ActionType.AbilityThree:
                    AbilityThree = enabled;
                    break;
                case ActionType.AbilityFour:
                    AbilityFour = enabled;
                    break;
                case ActionType.CharmAbility:
                    CharmAbility = enabled;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        public bool GetAction(ActionType actionType)
        {
            return actionType switch
            {
                ActionType.Shield => Shield,
                ActionType.BasicAttack => BasicAttack,
                ActionType.AbilityOne => AbilityOne,
                ActionType.AbilityTwo => AbilityTwo,
                ActionType.AbilityThree => AbilityThree,
                ActionType.AbilityFour => AbilityFour,
                ActionType.CharmAbility => CharmAbility,
                _ => throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null),
            };
        }
    }
}